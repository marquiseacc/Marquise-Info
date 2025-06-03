using Hangfire;
using Hangfire.Storage;
using Marquise_Web.Service.IService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Linq;
using System.Text;
using Unity;

[assembly: OwinStartup(typeof(Marquise_Web.UI.Startup))]

namespace Marquise_Web.UI
{
    public class Startup
    {
        [Obsolete]
        public void Configuration(IAppBuilder app)
        {
            // کلید و پارامترهای JWT
            var secretKey = "ThisIsA32CharLongSecretKeyForHS256!!"; // حداقل 32 کاراکتر
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // فعال‌سازی احراز هویت JWT
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

            // استفاده از Unity Container
            //IUnityContainer unityContainer = UnityConfig.Container ?? throw new InvalidOperationException("Unity container not initialized.");

            //// تنظیم Hangfire با SQL Server و Unity Activator
            //GlobalConfiguration.Configuration
            //    .UseSqlServerStorage("Marquise_WebEntities")
            //    .UseActivator(new HangfireActivator(unityContainer));

            //app.UseHangfireDashboard("/hangfire"); // داشبورد Hangfire
            //app.UseHangfireServer();              // فعال‌سازی سرور Hangfire

            //// ثبت Recurring Job فقط در صورتی که قبلاً وجود نداشته باشد
            //var jobExists = JobStorage.Current.GetConnection()
            //    .GetRecurringJobs()
            //    .Any(j => j.Id == "sync-accounts-job");

            //if (!jobExists)
            //{
            //    RecurringJob.AddOrUpdate<IUpdateService>(
            //        "sync-accounts-job",
            //        service => service.SyncAccountsToWebsiteAsync(),
            //        Cron.Daily(7, 0), // ساعت ۹:۴۰ صبح هر روز
            //        TimeZoneInfo.Local // یا TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time") برای دقت بیشتر
            //    );
            //}
        }
    }

    // Hangfire Activator برای Unity DI
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
