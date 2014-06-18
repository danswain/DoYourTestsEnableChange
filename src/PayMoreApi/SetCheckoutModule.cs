using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Dapper;
using Nancy;
using Nancy.Responses;

namespace PayMoreApi
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

            Get["/checkout/{checkoutid}"] = parameters =>
            {
                dynamic model = new ExpandoObject();
                var checkoutId = parameters.checkoutid;
                Guid sessionId = Guid.Empty;
                Guid.TryParse(checkoutId, out sessionId);

                if (sessionId == Guid.Empty)
                    return HttpStatusCode.NotFound;

                model.SessionId = sessionId;
                
                return View["Step", model];

            };

            Get["/"] = _ => View["Index"];

            Post["/login"] = _ =>
            {

                dynamic model = new ExpandoObject();
                model.Email = Request.Form.Email;
                model.Password = Request.Form.Password;
                model.SessionId = Request.Form.SessionId;

                if(model.Email == "test@test.com" && model.Password == "password")
                    return View["Step2", model];

                return View["Step", model];
            };

            Post["/confirm-payment"] = _ =>
            {                
                string sessionId = Request.Form["SessionId"];
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayMore"].ConnectionString))
                {
                    sqlConnection.Open();

                    var returnUrl = sqlConnection.Query<string>("SELECT ReturnUrl FROM PendingTransaction WHERE SessionId = @SessionId", new {SessionId = Guid.Parse(sessionId)}).SingleOrDefault();

                    sqlConnection.Close();

                    return new RedirectResponse(returnUrl, RedirectResponse.RedirectType.SeeOther);


                }
                
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

    public class PendingTransaction
    {
        public Guid SessionId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    public class CheckoutParameters
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}