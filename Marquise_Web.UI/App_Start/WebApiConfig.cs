using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Marquise_Web.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
            name: "CRM_Area",
            routeTemplate: "api/CRM/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
            config.Routes.MapHttpRoute(
                          name: "DefaultApiWithName",
                          routeTemplate: "api/{controller}/{action}/{id}",
                          defaults: new { id = RouteParameter.Optional }
        );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
