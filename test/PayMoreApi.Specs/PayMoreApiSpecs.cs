using NUnit.Framework;
using PayMoreApi.Models;

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
}