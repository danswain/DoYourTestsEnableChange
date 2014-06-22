using Nancy;

namespace PayMoreApi.Modules
{
    public class StatusModule : NancyModule
    {
        public StatusModule()
        {
            Get["paymore/status"] = _ => HttpStatusCode.OK;
        }
    }
}