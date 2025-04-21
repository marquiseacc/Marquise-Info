using Marquise_Web.Data;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(Marquise_Web.UI.Startup))] // تغییر YourNamespace به نام فضای نام پروژه شما

namespace Marquise_Web.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // تنظیمات OWIN — مثلاً Cookie Authentication
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/CRM/Auth/SendOtp"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Identity config — اگه داری
        }
    }
}