using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Nancy;
using Nancy.Responses;
using PayMoreApi.Models;

namespace PayMoreApi.Modules
{
    public class ConfirmPaymentModule : NancyModule
    {
        public ConfirmPaymentModule()
        {
            Post["/confirm-payment"] = _ =>
            {
                string sessionId = Request.Form["SessionId"];
                string payOrCancel = Request.Form["PaymentAction"];
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayMore"].ConnectionString)
                    )
                {
                    sqlConnection.Open();

                    var paymentAction =
                        sqlConnection.Query<PaymentAction>(
                            "SELECT CancelUrl,ReturnUrl FROM PendingTransaction WHERE SessionId = @SessionId",
                            new {SessionId = Guid.Parse(sessionId)}).SingleOrDefault();

                    sqlConnection.Close();

                    switch (payOrCancel.ToLower())
                    {
                        case "pay":
                            return new RedirectResponse(paymentAction.ReturnUrl + "?auth-code=" + Guid.NewGuid().ToString(),
                                RedirectResponse.RedirectType.SeeOther);
                        case "cancel":
                            return new RedirectResponse(paymentAction.CancelUrl, RedirectResponse.RedirectType.SeeOther);
                    }

                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }
}