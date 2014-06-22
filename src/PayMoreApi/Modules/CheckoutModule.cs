using System;
using System.Dynamic;
using Nancy;

namespace PayMoreApi.Modules
{
    public class CheckoutModule : NancyModule
    {
        public void Checkout()
        {
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
        }
    }
}