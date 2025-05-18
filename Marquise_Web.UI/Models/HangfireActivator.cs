using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace Marquise_Web.UI.Models
{
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
    public class UnityJobActivator : JobActivator
    {
        private readonly IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.Resolve(jobType);
        }
    }
}