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

namespace Marquise_Web.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWorkRepository, UnitOfWorkRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessageRepository, MessageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository,UserRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IUnitOfWorkService, UnitOfWorkService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessageService, MessageService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthService,  AuthService>(new HierarchicalLifetimeManager());


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