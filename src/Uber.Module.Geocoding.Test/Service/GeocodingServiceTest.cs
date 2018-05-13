using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Uber.Module.Geocoding.Test.Service
{
    [Collection(GeocodingTestCollection.Name)]
    public class GeocodingServiceTest : GeocodingTestBase
    {
        public GeocodingServiceTest(GeocodingFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            GeocodingService.Query().Should().NotBeNull();
        }

        [Fact]
        public async Task CanQuerySingle()
        {
            GeocodingService.QuerySingle(Guid.NewGuid()).Should().BeEmpty();

            var address = await GeocodingService.Resolve("1 Microsoft Way, Redmond, WA 98052, USA");
            GeocodingService.QuerySingle(address.Key).Should().HaveCount(1);
        }
    }
}
