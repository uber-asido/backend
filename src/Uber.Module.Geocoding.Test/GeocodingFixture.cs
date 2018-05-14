using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Uber.Core.Test;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.EFCore;
using Xunit;

namespace Uber.Module.Geocoding.Test
{
    public class GeocodingFixture : ServiceFixture
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=192.168.110.130;Port=5432;Database=uber_geocoding_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddGeocoding(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));

            var geocodeProviderMock = new Mock<IGeocodeProvider>(MockBehavior.Strict);
            geocodeProviderMock
                .Setup(e => e.Geocode(It.IsAny<string>()))
                .Returns((string address) =>
                {
                    var geocode = new Geocode
                    {
                        FormattedAddress = $"Mocked formatted address '{address}'",
                        Latitude = 1,
                        Longitude = 1
                    };
                    return Task.FromResult(geocode);
                });
            services.AddSingleton(geocodeProviderMock.Object);
        }
    }

    [CollectionDefinition(Name)]
    public class GeocodingTestCollection : ICollectionFixture<GeocodingFixture>
    {
        public const string Name = "Geocoding test collection";
    }
}
