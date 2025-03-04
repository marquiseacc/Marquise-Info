using AutoMapper;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Model.SiteModel;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using Utilities.Map;

namespace Marquise_Web.UI
{
    public static class UnityConfig
    {
        

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IMessageRepository, MessageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICRMAccountRepository, CRMAccountRepository>(new HierarchicalLifetimeManager());


            container.RegisterType<IEmailService, EmailService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICRMAccountService, CRMAccountService>(new HierarchicalLifetimeManager());


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
                cfg.AddProfile(new CRMAutoMapperConfiguration());
                cfg.AddProfile(new SiteAutoMapperConfiguration());
            });

            config.CompileMappings(); 
            config.AssertConfigurationIsValid(); 
            container.RegisterInstance(config.CreateMapper());

            container.RegisterFactory<SmtpSettings>(c => new SmtpSettings
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
                Username = ConfigurationManager.AppSettings["SmtpUsername"],
                Password = ConfigurationManager.AppSettings["SmtpPassword"],
                From = ConfigurationManager.AppSettings["SmtpFrom"]
            }, new HierarchicalLifetimeManager());

            container.RegisterType<HttpClient>(new HierarchicalLifetimeManager());



            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}