using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace PayMoreApi.Specs
{
    [TestFixture]
    public class PayMoreApiSpecs
    {
        [TestCase(null)]
        [TestCase("4543HiTHERE00000")]
        [TestCase("HELLO WORLD")]
        [TestCase("4543")]        
        [TestCase("0000")]        
        [TestCase("45434543454345-1")]                
        public void Invalid_card_numbers_should_fail_validation(string cardNumber)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = cardNumber,
                CardHolderName = "Mr Dan Swain",
                StartDate = "6/13",
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.Throws<CardNotValid>(() => new Card().Register(cardDetails));
        }

        [TestCase("4543454345434543")]
        [TestCase("4543000000000000")]
        public void Valid_card_numbers_should_pass_validation(string cardNumber)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = cardNumber,
                CardHolderName = "MR DAN SWAIN",
                StartDate = "6/13",
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.DoesNotThrow(()=>new Card().Register(cardDetails));            
        }

        [TestCase("Dan Swain")]
        [TestCase("DAN SWAIN")]
        [TestCase("Mr DAN SWAIN")]
        [TestCase("DAVE ROBERT SMITH")]
        [TestCase("DR BECCI SWAIN")]
        [TestCase("mr dan swain")]
        [TestCase("Mr Dan Swain")]
        public void Invalid_card_holder_names_should_fail_validation(string cardHolderName)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = cardHolderName,
                StartDate = "6/13",
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.Throws<CardNotValid>(() => new Card().Register(cardDetails));
        }

        [TestCase("SIR DAN SWAIN")]
        [TestCase("LORD LEON HEWITT")]
        [TestCase("EMPEROR LEON HEWITT")]
        [TestCase("EMPRESS BECCI SWAIN")]
        public void Valid_cardholder_names_should_pass_validation(string cardHolderName)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = cardHolderName,
                StartDate = "6/13",
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.DoesNotThrow(() => new Card().Register(cardDetails));
        }

        [TestCase(null)]        
        [TestCase("")]        
        [TestCase("07/2014")]        
        [TestCase("07/14")]        
        [TestCase("7/2014")]                                    
        [TestCase("31/12")]                                    
        [TestCase("1/99")]                                    
        [TestCase("19/13")]                                    
        public void Invalid_start_dates_should_fail_validation(string startDate)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = "SIR DAN SWAIN",
                StartDate = startDate,
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.Throws<CardNotValid>(() => new Card().Register(cardDetails));
        }

        [TestCase("7/14")]    
        [TestCase("1/14")]    
        [TestCase("10/12")]    
        [TestCase("12/12")]            
        public void valid_start_dates_should_pass_validation(string startDate)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = "LORD LEON HEWITT",
                StartDate = startDate,
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.DoesNotThrow(() => new Card().Register(cardDetails));
        }  
        
        
        [TestCase(null)]        
        [TestCase("")]        
        [TestCase("07/2014")]        
        [TestCase("07/14")]        
        [TestCase("7/2014")]                                    
        [TestCase("31/12")]                                    
        [TestCase("1/99")]                                    
        [TestCase("19/13")]
        [TestCase("12/16")]                                       
        public void Invalid_end_dates_should_fail_validation(string endDate)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = "SIR DAN SWAIN",
                StartDate = endDate,
                EndDate = "6/14",
                CVV = "322",
                CardType = "Credit"

            };

            Assert.Throws<CardNotValid>(() => new Card().Register(cardDetails));
        }

        [TestCase("7/14")]    
        [TestCase("1/14")]    
        [TestCase("10/12")]    
        [TestCase("12/12")]            
        [TestCase("12/15")]                    
        public void valid_end_dates_should_pass_validation(string endDate)
        {
            var cardDetails = new CardDetails
            {
                CardNumber = "4543454345434543",
                CardHolderName = "LORD LEON HEWITT",
                StartDate = "6/14",
                EndDate = endDate,
                CVV = "322",
                CardType = "Credit"

            };

            Assert.DoesNotThrow(() => new Card().Register(cardDetails));
        }
    }






    public class CardNotValid : Exception
    {
        public CardNotValid(string message) : base(message)
        {
        }

    }

    public class Card
    {
        public Guid Register(CardDetails cardDetails)
        {
            ValidateCardNumber(cardDetails);

            ValidateCardHolderName(cardDetails);

            ValidateStartAndEndDate(cardDetails);


            return Guid.NewGuid();
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

    public class CardDetails
    {
        public string CVV { get; set; }

        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderName { get; set; }

        public string StartDate { get; set; }

        public string ExpiryDate { get; set; }

        public string IssueNumber { get; set; }
        public string EndDate { get; set; }
    }

}