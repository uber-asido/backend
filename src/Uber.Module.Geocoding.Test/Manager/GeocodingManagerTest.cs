using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Uber.Module.Geocoding.Test.Manager
{
    [Collection(GeocodingTestCollection.Name)]
    public class GeocodingManagerTest : GeocodingServiceTest
    {
        public GeocodingManagerTest(GeocodingFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            GeocodingManager.Query().Should().NotBeNull();
        }

        [Fact]
        public async Task CanQuerySingle()
        {
            GeocodingManager.QuerySingle(Guid.NewGuid()).Should().BeEmpty();

            var address = await GeocodingManager.Resolve("1 Microsoft Way, Redmond, WA 98052, USA");
            GeocodingManager.QuerySingle(address.Key).Should().HaveCount(1);
        }
    }
}
