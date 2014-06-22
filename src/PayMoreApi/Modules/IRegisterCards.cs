using PayMoreApi.Models;

namespace PayMoreApi.Modules
{
    public interface IRegisterCards
    {
        RegisteredCard Register(CardDetails cardDetails);
    }

    public class RegisteredCard
    {
        public string Type { get; set; }

        public string Number { get; set; }

        public string CardHolderName { get; set; }

        public string ExpiryDate { get; set; }
    }
}