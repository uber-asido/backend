using Microsoft.Extensions.DependencyInjection;
using Uber.Module.Geocoding.Abstraction.Service;

namespace Uber.Module.Geocoding.Test
{
    public class GeocodingTestBase
    {
        public readonly IGeocodingService GeocodingService;

        private readonly IServiceScope scope;

        public GeocodingTestBase(GeocodingFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            GeocodingService = scope.ServiceProvider.GetRequiredService<IGeocodingService>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
