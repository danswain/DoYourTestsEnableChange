using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace PayMoreApi
{
    public class StatusModule : NancyModule
    {
        public StatusModule()
        {
            Get["/status"] = _ => HttpStatusCode.OK;
        }
    }
}