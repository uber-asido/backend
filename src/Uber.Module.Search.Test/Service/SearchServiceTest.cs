using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;
using Xunit;

namespace Uber.Module.Search.Test.Service
{
    [Collection(SearchTestCollection.Name)]
    public class SearchServiceTest : SearchTestBase
    {
        public SearchServiceTest(SearchFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            SearchService.Query().Should().NotBeNull();
        }

        [Fact]
        public async Task CanQuerySingle()
        {
            SearchService.QuerySingle(Guid.NewGuid()).Should().BeEmpty();

            var item = new SearchItem { Key = Guid.NewGuid(), Text = "foobar", Type = SearchItemType.Organization };
            await SearchService.Create(item);
            SearchService.QuerySingle(item.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanCreate()
        {
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = "first", Type = SearchItemType.Organization };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = "second", Type = SearchItemType.Person };

            await SearchService.Create(item1);
            await SearchService.Create(item2);

            var foundItem1 = SearchService.QuerySingle(item1.Key).SingleOrDefault();
            var foundItem2 = SearchService.QuerySingle(item2.Key).SingleOrDefault();

            foreach (var pair in new[] { (item1, foundItem1), (item2, foundItem2) })
            {
                var item = pair.Item1;
                var foundItem = pair.Item2;

                foundItem.Should().NotBeNull();
                foundItem.Key.Should().Be(item.Key);
                foundItem.Text.Should().Be(item.Text);
                foundItem.Type.Should().Be(item.Type);
            }
        }
    }
}
