using Microsoft.EntityFrameworkCore;
using System;
using Uber.Core.EFCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataStore<TDataStore, TDataContext>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDataStore : DataStoreBase<TDataContext>
            where TDataContext : DbContext
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.Add(ServiceDescriptor.Describe(typeof(TDataStore), typeof(TDataStore), lifetime));
            return services;
        }
    }
}
