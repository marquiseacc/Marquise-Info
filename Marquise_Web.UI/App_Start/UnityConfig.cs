﻿using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Data.Repository;
using Marquise_Web.Model.DTOs.SiteModel;
using Marquise_Web.Model.Entities;
using Marquise_Web.Service.IService;
using Marquise_Web.Service.Service;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System.Web;
using System;
using Unity.AspNet.Mvc;
using Marquise_Web.Utilities.Messaging;


namespace Marquise_Web.UI
{
    public static class UnityConfig
    {
        private static UnityContainer container;
        public static void RegisterComponents()
        {
            container = new UnityContainer();

            container.RegisterType<IUnitOfWorkRepository, UnitOfWorkRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessageRepository, MessageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IUnitOfWorkService, UnitOfWorkService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessageService, MessageService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthService, AuthService>(new HierarchicalLifetimeManager());

            container.RegisterType<ISmsSender, SmsSender>(new HierarchicalLifetimeManager());


            container.RegisterFactory<SmtpSettings>(c => new SmtpSettings
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
                Username = ConfigurationManager.AppSettings["SmtpUsername"],
                Password = ConfigurationManager.AppSettings["SmtpPassword"],
                From = ConfigurationManager.AppSettings["SmtpFrom"]
            }, new HierarchicalLifetimeManager());

            container.RegisterType<HttpClient>(new HierarchicalLifetimeManager());

            // ثبت IdentityDbContext
            container.RegisterType<Marquise_WebEntities>(new HierarchicalLifetimeManager());

            // ثبت UserManager و SignInManager
            // UserStore
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<Marquise_WebEntities>())
            );

            container.RegisterFactory<ApplicationUserManager>(c =>
            {
                var dbContext = c.Resolve<Marquise_WebEntities>();
                var userStore = new UserStore<ApplicationUser>(dbContext);
                return new ApplicationUserManager(userStore);
            });



            container.RegisterFactory<ApplicationSignInManager>(c =>
            {
                var httpContext = HttpContext.Current;
                if (httpContext == null)
                    throw new InvalidOperationException("HttpContext.Current is null — this code must run inside an HTTP request.");

                var owinEnvironment = httpContext.Items["owin.Environment"];
                if (owinEnvironment == null)
                    throw new InvalidOperationException("OWIN environment is not initialized — request is not processed via OWIN pipeline.");

                var owinContext = httpContext.GetOwinContext();

                var userManager = c.Resolve<ApplicationUserManager>();
                var authManager = owinContext.Authentication;

                return new ApplicationSignInManager(userManager, authManager);
            }, new PerRequestLifetimeManager());


            DependencyResolver.SetResolver(new Unity.AspNet.Mvc.UnityDependencyResolver(container));

            // برای Web API
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}