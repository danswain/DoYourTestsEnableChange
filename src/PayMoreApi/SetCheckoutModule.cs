using System;
using System.Collections.Generic;
using System.Dynamic;
using Nancy;
using Nancy.Responses;

namespace PayMoreApi
{
    public class SetCheckoutModule : NancyModule
    {
        static Dictionary<Guid,CheckoutParameters> _checkoutRequest = new Dictionary<Guid, CheckoutParameters>();

        public SetCheckoutModule()
        {
            Get["/SetCheckout"] = parameters =>
            {                
                var returnUrl = Request.Query.returnUrl;
                var cancelUrl = Request.Query.cancelUrl;

                if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(cancelUrl))
                    return HttpStatusCode.BadRequest;

                var requestKey = Guid.NewGuid();

                _checkoutRequest.Add(requestKey, new CheckoutParameters
                {
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                });
                var redirectUrl = string.Format("/checkout/{0}",requestKey);

                return new RedirectResponse(redirectUrl, RedirectResponse.RedirectType.SeeOther);
            };

            Get["/checkout/{checkoutid}"] = parameters =>
            {
                dynamic model = new ExpandoObject();
                model.SessionId = parameters.checkoutid;

                return View["Step", model];

            };

            Get["/"] = _ => View["Index"];

            Post["/login"] = _ =>
            {

                dynamic model = new ExpandoObject();
                model.Email = Request.Form.Email;
                model.Password = Request.Form.Password;
                model.SessionId = Request.Form.SessionId;

                return View["Step2", model];
            };
        } 
    }

    public class CheckoutParameters
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}