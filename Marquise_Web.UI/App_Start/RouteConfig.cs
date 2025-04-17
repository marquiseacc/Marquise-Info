using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Marquise_Web.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
            name: "CRM",
            url: "CRM/{controller}/{action}/{id}",
            defaults: new { controller = "Auth", action = "SendOtp", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
            foreach (Route route in routes.OfType<Route>())
            {
                System.Diagnostics.Debug.WriteLine($"URL: {route.Url}");
            }
        }
    }
}
