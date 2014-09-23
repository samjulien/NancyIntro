using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace NancyIntro
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Func<Request, bool> _isNotApiClient = request => !request.Headers.UserAgent.ToLower().StartsWith("curl");

            Get["/", ctx=> _isNotApiClient.Invoke(ctx.Request)] = p => View["index.html"];

            Get["/"] = p => "Welcome to the Pluralsight API!";

        }
    }
}