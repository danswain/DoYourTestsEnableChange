using System.Dynamic;
using Nancy;

namespace PayMoreApi.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Post["/login"] = _ =>
            {
                dynamic model = new ExpandoObject();
                model.Email = Request.Form.Email;
                model.Password = Request.Form.Password;
                model.SessionId = Request.Form.SessionId;

                if (model.Email == "test@test.com" && model.Password == "password")
                    return View["Step2", model];

                return View["Step", model];
            };
        }
    }
}