using System;
using Uber.Module.Geocoding.Abstraction;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.Service;

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
                .AddService<IGeocodingService, GeocodingService>();

            var builder = new GeocodingBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
