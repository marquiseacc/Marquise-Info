using Hangfire;
using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Net.Http;

[assembly: OwinStartup(typeof(Marquise_Web.UI.Startup))] // تغییر YourNamespace به نام فضای نام پروژه شما

namespace Marquise_Web.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ---------------------------
            // 🟡 Identity Configuration
            // ---------------------------
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/CRM/Auth/SendOtp"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // ---------------------------
            // 🟢 Dependency Injection
            // ---------------------------
            var services = new ServiceCollection();
            services.AddTransient<ApplicationDbContext>(); // ✅ برای UserRepository

            services.AddTransient<Marquise_WebEntities>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UnitOfWorkRepository>();
            services.AddTransient<HttpClient>();

            services.AddTransient<CRMApiSetting>(provider => new CRMApiSetting
            {
                ApiBaseUrl = "https://your-crm-api-url/"
            });

            services.AddTransient<IAccountService, AccountService>();

            var serviceProvider = services.BuildServiceProvider();

            // ---------------------------
            // 🔵 Hangfire
            // ---------------------------
            //GlobalConfiguration.Configuration
            //    .UseSqlServerStorage("Marquise_WebEntities")
            //    .UseActivator(new UnityJobActivator(UnityConfig.Container)); // این خیلی مهمه


            //app.UseHangfireDashboard();
            //app.UseHangfireServer();

            //RecurringJob.AddOrUpdate<IAccountService>(
            //    "sync-accounts-job",
            //    service => service.SyncAccountsToWebsiteAsync(),
            //    Cron.Daily(7, 0));
        }
    }

    // ---------------------------
    // 🔧 Hangfire DI Activator
    // ---------------------------
    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetService(jobType);
        }
    }
}