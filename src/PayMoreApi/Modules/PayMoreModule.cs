using Nancy;

namespace PayMoreApi.Modules
{
    public class PayMoreModule : NancyModule
    {
        public PayMoreModule()
        {            
            Post["paymore/card"] = parameters =>
            {
                return HttpStatusCode.Created;
            };
        }
    }
}