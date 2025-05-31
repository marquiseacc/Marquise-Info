using Hangfire;
using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Model.DTOs.CRM;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Net.Http;
using System.Text;

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
                AuthenticationMode = AuthenticationMode.Active,
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

            // Cookie Authentication کامنت شده چون فقط JWT استفاده می‌شود
            /*
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/CRM/Auth/SendOtp"),
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true,
                CookieSecure = CookieSecureOption.SameAsRequest,
                CookieHttpOnly = true,
                CookieSameSite = Microsoft.Owin.SameSiteMode.Lax
            });
            */

            // DI container
            var services = new ServiceCollection();

            services.AddTransient<ApplicationDbContext>();
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

            // Hangfire اگر لازم نیست کامنت باشه
            /*
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Marquise_WebEntities")
                .UseActivator(new HangfireActivator(serviceProvider));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<IAccountService>(
                "sync-accounts-job",
                service => service.SyncAccountsToWebsiteAsync(),
                Cron.Daily(7, 0));
            */
        }
    }

    // Hangfire Activator
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
