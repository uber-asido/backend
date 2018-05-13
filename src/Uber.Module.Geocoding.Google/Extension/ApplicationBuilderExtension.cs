using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.Google;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGeocodingGoogle(this IServiceCollection services, string googleApiKey)
        {
            services.AddSingleton<IGeocodeProvider>(serviceProvider => new GoogleGeocodeProvider(googleApiKey));
            return services;
        }
    }
}
