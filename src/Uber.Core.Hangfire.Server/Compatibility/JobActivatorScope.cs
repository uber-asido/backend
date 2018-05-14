using Microsoft.Extensions.DependencyInjection;
using System;

namespace Uber.Core.Hangfire.Server.Compatibility
{
    public class JobActivatorScope : global::Hangfire.JobActivatorScope
    {
        private readonly IServiceScope serviceScope;
        private readonly IServiceProvider serviceProvider;

        public JobActivatorScope(IServiceScope serviceScope)
        {
            this.serviceScope = serviceScope;
            serviceProvider = serviceScope.ServiceProvider;
        }

        public override void DisposeScope()
        {
            serviceScope.Dispose();
        }

        public override object Resolve(Type type)
        {
            return serviceProvider.GetService(type);
        }
    }
}
