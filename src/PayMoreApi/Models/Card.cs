using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using PayMoreApi.Modules;

namespace PayMoreApi.Models
{
    public class Card : IRegisterCards
    {
        private readonly IStoreCards _cardRepository;

        public Card(IStoreCards cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card()
        {
            
        }

        public RegisteredCard Register(CardDetails cardDetails)
        {
            ValidateCardNumber(cardDetails);

            ValidateCardHolderName(cardDetails);

            ValidateStartAndEndDate(cardDetails);

            _cardRepository.Save(cardDetails);

            return new RegisteredCard
            {
                CardHolderName = cardDetails.CardHolderName,
                ExpiryDate = cardDetails.ExpiryDate,
                Number = "1234",
                Type = cardDetails.CardType
            };
        }

        private static void ValidateStartAndEndDate(CardDetails cardDetails)
        {
            var theCurrentYear = int.Parse(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture).Remove(0, 2));

            ValidateDate(cardDetails.StartDate, theCardYearCannotExceed: theCurrentYear);
            ValidateDate(cardDetails.EndDate, theCardYearCannotExceed: 15);

        }

        private static void ValidateDate(string inputDate, int theCardYearCannotExceed = 14)
        {
            if (string.IsNullOrEmpty(inputDate))
                throw new CardNotValid("Start Date is not valid");

            var dateRegex = new Regex(@"^(?<month>[1]?[0-9])/(?<year>[0-9]{2})$");

            if (!dateRegex.IsMatch(inputDate))
                throw new CardNotValid("Date is not valid");

            var date = dateRegex.Match(inputDate);

            var month = date.Groups["month"].Value;
            int dateMonth;
            int.TryParse(month, out dateMonth);
            if (dateMonth > 12)
                throw new CardNotValid("Date is not Valid");


            var year = date.Groups["year"].Value;
            int dateYear;
            int.TryParse(year, out dateYear);

            if (dateYear > theCardYearCannotExceed)
                throw new CardNotValid("Date is not valid");
        }

        private static void ValidateCardHolderName(CardDetails cardDetails)
        {
            if (string.IsNullOrEmpty(cardDetails.CardHolderName))
                throw new CardNotValid("Cardholder Name is not valid");

            var validCardHolderName = new Regex(@"^(?<salutation>[A-Z]{2,7}) (?<firstName>[A-Z]{3,16}) (?<surname>[A-Z]{3,16})$");

            if (!validCardHolderName.IsMatch(cardDetails.CardHolderName))
                throw new CardNotValid("Cardholder Name is not valid");

            var cardHolderName = validCardHolderName.Match(cardDetails.CardHolderName);

            var salutation = cardHolderName.Groups["salutation"].Value;

            var validSalutations = new[]
            {"MR", "MRS", "MISS", "MS", "PROF", "LORD", "SIR", "LADY", "DAME", "BARON", "EMPEROR", "EMPRESS"};

            if (!validSalutations.Contains(salutation))
                throw new CardNotValid("Cardholder Name is not valid");
        }

        private static void ValidateCardNumber(CardDetails cardDetails)
        {
            if (string.IsNullOrEmpty(cardDetails.CardNumber))
                throw new CardNotValid("Card Number is not valid");

            var validCardNumber = new Regex(@"[\d]{16}");

            if (!validCardNumber.IsMatch(cardDetails.CardNumber))
                throw new CardNotValid("Card Number is not valid");

            if (!cardDetails.CardNumber.StartsWith("4543"))
                throw new CardNotValid("Card Number is not valid");
        }
    }
}