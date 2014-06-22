using System;

namespace PayMoreApi.Modules
{
    public class PendingTransaction
    {
        public Guid SessionId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}