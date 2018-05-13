using Microsoft.Extensions.DependencyInjection;
using Uber.Module.Geocoding.Abstraction.Manager;

namespace Uber.Module.Geocoding.Test
{
    public class GeocodingServiceTest
    {
        public readonly IGeocodingManager GeocodingManager;

        private readonly IServiceScope scope;

        public GeocodingServiceTest(GeocodingFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            GeocodingManager = scope.ServiceProvider.GetRequiredService<IGeocodingManager>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
