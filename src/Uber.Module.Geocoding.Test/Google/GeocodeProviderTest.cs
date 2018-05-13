using FluentAssertions;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Google;
using Xunit;

namespace Uber.Module.Geocoding.Test.Google
{
    public class GeocodeProviderTest
    {
        [Fact]
        public async Task CanGeocodeLocation()
        {
            var google = new GoogleGeocodeProvider("AIzaSyBa8GWt6bsqItvQGTAFBLlY5WkX6Zwp73I");
            var geocode = await google.Geocode("skanderborg");

            geocode.Should().NotBeNull();
            geocode.FormattedAddress.Should().Be("8660 Skanderborg, Denmark");
            geocode.Latitude.Should().Be(56.037247);
            geocode.Longitude.Should().Be(9.9297989);
        }
    }
}
