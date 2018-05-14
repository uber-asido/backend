using Microsoft.Extensions.DependencyInjection;
using System;

namespace Uber.Core.Hangfire.Server.Compatibility
{
    public class JobActivator : global::Hangfire.JobActivator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public JobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        public override global::Hangfire.JobActivatorScope BeginScope(global::Hangfire.JobActivatorContext context)
        {
            return new JobActivatorScope(serviceScopeFactory.CreateScope());
        }

        public override object ActivateJob(Type jobType)
        {
            return base.ActivateJob(jobType);
        }
    }
}
