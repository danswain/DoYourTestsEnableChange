using Nancy;

namespace PayMoreApi.Modules
{
    public class IndexModule : NancyModule
    {
        public void Index()
        {
            Get["/"] = _ => View["Index"];
        }
    }
}