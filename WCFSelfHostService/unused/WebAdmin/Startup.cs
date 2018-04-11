using Owin;
using System.Web.Http;
using System.Web.Http.SelfHost; 

namespace VTCTIManagerService.WebAdmin
{
    class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "rest/{controller}/{function}",
                defaults: new { id = RouteParameter.Optional }            
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "rest/{controller}/{function}/{id}/{idd}",
                defaults: new { id = RouteParameter.Optional, idd = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "DefaultPages",
            routeTemplate: "{page}",
            defaults: new { controller = "Html" }
            );

            config.Routes.MapHttpRoute(
                    name: "CatchAll",
                    routeTemplate: "{*catchall}",
                    defaults: new { controller = "Html", id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        } 
    }
}
