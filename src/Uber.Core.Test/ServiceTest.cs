using Microsoft.Extensions.DependencyInjection;
using System;

namespace Uber.Core.Test
{
    public abstract class ServiceTest : IDisposable
    {
        private static IServiceProvider serviceProvider { get; set; }

        public ServiceTest()
        {
            var services = new ServiceCollection();
            services.AddInstaller();
            ConfigureServices(services);
            serviceProvider = new DependencyInjectionContainer().Populate(services);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected abstract void ConfigureServices(IServiceCollection services);
    }
}
