using System;

namespace PayMoreApi.Models
{
    public class CardNotValid : Exception
    {
        public CardNotValid(string message) : base(message)
        {
        }

    }
}