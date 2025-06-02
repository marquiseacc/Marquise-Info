using Hangfire;
using Marquise_Web.Service.IService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Text;
using Unity;

[assembly: OwinStartup(typeof(Marquise_Web.UI.Startup))]

namespace Marquise_Web.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // کلید و پارامترهای JWT
            var secretKey = "ThisIsA32CharLongSecretKeyForHS256!!"; // حداقل 32 کاراکتر
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "MarquiseSupport",
                    ValidAudience = "MarquiseSupport",
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }
            });

            // استفاده از کانتینر Unity که در UnityConfig ساخته شده
            IUnityContainer unityContainer = UnityConfig.Container ?? throw new InvalidOperationException("Unity container not initialized.");

            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Marquise_WebEntities")
                .UseActivator(new HangfireActivator(unityContainer));

            app.UseHangfireDashboard("/hangfire"); // آدرس داشبورد: /hangfire
            app.UseHangfireServer();

            // حذف job قبلی در صورت وجود، سپس ثبت مجدد آن
            RecurringJob.RemoveIfExists("sync-accounts-job");
            RecurringJob.AddOrUpdate<IUpdateService>(
                "sync-accounts-job",
                service => service.SyncAccountsToWebsiteAsync(),
                Cron.Daily(7, 0));
        }
    }

    // Hangfire Activator برای Unity
    public class HangfireActivator : JobActivator
    {
        private readonly IUnityContainer _container;

        public HangfireActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.Resolve(jobType);
        }
    }
}
