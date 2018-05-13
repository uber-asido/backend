using System;
using Uber.Module.Geocoding.Abstraction;
using Uber.Module.Geocoding.Abstraction.Manager;
using Uber.Module.Geocoding.Manager;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class GeocodingBuilder : IGeocodingBuilder
    {
        public IServiceCollection Services { get; }

        public GeocodingBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGeocoding(this IServiceCollection services, Action<IGeocodingBuilder> configureAction)
        {
            services
                .AddManager<IGeocodingManager, GeocodingManager>();

            var builder = new GeocodingBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
