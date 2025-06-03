using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Marquise_Web.UI.APIController;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
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


        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;

            int httpCode = 500; // پیش‌فرض
            if (httpException != null)
            {
                httpCode = httpException.GetHttpCode();
            }

            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = httpCode;

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            routeData.Values["code"] = httpCode;

            IController controller = new Marquise_Web.UI.Controllers.ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

    }
}
