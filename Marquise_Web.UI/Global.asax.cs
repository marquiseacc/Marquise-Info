using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.APIController;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Microsoft.Practices.Unity;
using Unity.Configuration;
using Unity.WebApi;




namespace Marquise_Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
    }
}
