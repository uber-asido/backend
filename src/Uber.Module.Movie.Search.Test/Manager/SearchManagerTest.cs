using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Search.Abstraction.Model;
using Xunit;

namespace Uber.Module.Movie.Search.Test.Manager
{
    [Collection(SearchTestCollection.Name)]
    public class SearchManagerTest : SearchTestBase
    {
        public SearchManagerTest(SearchFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            SearchManager.Query().ToList().Should().NotBeNull();
        }

        [Fact]
        public void CanQuerySingle()
        {
            SearchManager.QuerySingle(Guid.NewGuid()).Should().BeEmpty();

            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = "foobar", Type = SearchItemType.Organization };
            SearchManager.QuerySingle(item1.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanCreate()
        {
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = "first", Type = SearchItemType.Organization };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = "second", Type = SearchItemType.Person };

            await SearchManager.Create(item1);
            await SearchManager.Create(item2);

            var foundItem1 = SearchManager.QuerySingle(item1.Key).SingleOrDefault();
            var foundItem2 = SearchManager.QuerySingle(item2.Key).SingleOrDefault();

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
