using Grace.DependencyInjection;
using Grace.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Core.Setup;

namespace Uber.Core.Test
{
    public abstract class ServiceFixture
    {
        public readonly IServiceProvider RootServiceProvider;

        public ServiceFixture()
        {
            var services = new ServiceCollection();
            services.AddInstaller();
            ConfigureServices(services);
            RootServiceProvider = new DependencyInjectionContainer().Populate(services);
            Configure();

            using (var scope = RootServiceProvider.CreateScope())
            {
                var installer = scope.ServiceProvider.GetRequiredService<Installer>();
                installer.Execute().Wait();
            }
        }

        protected abstract void ConfigureServices(IServiceCollection services);
        protected abstract void Configure();
    }
}
