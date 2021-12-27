using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(MyTurn.Web.Startup))]

namespace MyTurn.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below. JSONP requests are insecure but some older browsers (and some versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
