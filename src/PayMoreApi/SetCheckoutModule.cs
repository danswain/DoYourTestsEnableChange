using System;
using System.Collections.Generic;
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

            Get["/checkout/{checkoutid}"] = _ =>
            {
                return View["Step", new CheckoutPageViewModel()];
            };
        } 
    }

    public class CheckoutParameters
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}