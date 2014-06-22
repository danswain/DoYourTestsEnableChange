using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Nancy;
using Nancy.Responses;

namespace PayMoreApi.Modules
{
    public class SetCheckoutModule : NancyModule
    {        
        public SetCheckoutModule()
        {
            Get["/SetCheckout"] = parameters =>
            {                
                var returnUrl = Request.Query.returnUrl;
                var cancelUrl = Request.Query.cancelUrl;

                if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(cancelUrl))
                    return HttpStatusCode.BadRequest;

                var pendingTransactionId = CreatePendingTransaction(returnUrl,cancelUrl);
                
                
                var redirectUrl = string.Format("/checkout/{0}",pendingTransactionId);

                return new RedirectResponse(redirectUrl, RedirectResponse.RedirectType.SeeOther);
            };
        }

        private Guid CreatePendingTransaction(string returnUrl, string cancelUrl)
        {
            var pendingTransactionSessionId = Guid.NewGuid();

            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayMore"].ConnectionString))
            {
                sqlConnection.Open();

                var pendingTransaction = new PendingTransaction
                {
                    SessionId = pendingTransactionSessionId,
                    ProductName = "ProductName",
                    Price = "100p",
                    Quantity = "2",
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                };

                var inserted = sqlConnection.Query(
                    @"
                            insert PendingTransaction(SessionId, ProductName, Price,Quantity, ReturnUrl, CancelUrl)
                            values (@SessionId, @ProductName, @Price, @Quantity, @ReturnUrl, @CancelUrl)
                            select cast(scope_identity() as int)
                        ", pendingTransaction).First();

                sqlConnection.Close();

                
            }

            return pendingTransactionSessionId;
        }
    }
}