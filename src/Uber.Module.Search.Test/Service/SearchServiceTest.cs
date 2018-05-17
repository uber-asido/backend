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
            var found = await SearchService.Merge(Guid.NewGuid(), item);
            SearchService.QuerySingle(found.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanMerge()
        {
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Organization };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };

            var merged = await SearchService.Merge(Guid.NewGuid(), new[] { item1, item2 });

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

        [Fact]
        public async Task CanMergeWithDuplicate()
        {
            var targetKey = Guid.NewGuid();
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };

            var found1 = await SearchService.Merge(targetKey, item1);
            var merged2 = await SearchService.Merge(targetKey, new[] { item1, item2 });
            var found2 = merged2.Single(e => e.Key != found1.Key);

            merged2.Should().HaveCount(2);
            merged2.Should().Contain(e => e.Key == item1.Key);
            merged2.Should().Contain(e => e.Key == item2.Key);
            merged2.Select(e => e.Key).Should().BeEquivalentTo(new[] { found1.Key, found2.Key });
            merged2.Select(e => e.Text).Should().BeEquivalentTo(new[] { found1.Text, found2.Text });
            merged2.Select(e => e.Type).Should().BeEquivalentTo(new[] { found1.Type, found2.Type });

            (await SearchService.Find(found1.Key)).Should().NotBeNull();
            (await SearchService.Find(found2.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMergeWithDuplicateWhenTypeDiffers()
        {
            var targetKey = Guid.NewGuid();
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = item1.Text, Type = SearchItemType.Organization };

            var found1 = await SearchService.Merge(targetKey, item1);
            var merged2 = await SearchService.Merge(targetKey, new[] { item1, item2 });
            var found2 = merged2.Single(e => e.Key != found1.Key);

            merged2.Should().HaveCount(2);
            merged2.Should().Contain(e => e.Key == item1.Key);
            merged2.Should().Contain(e => e.Key == item2.Key);
            merged2.Select(e => e.Key).Should().BeEquivalentTo(new[] { found1.Key, found2.Key });
            merged2.Select(e => e.Text).Should().BeEquivalentTo(new[] { found1.Text, found2.Text });
            merged2.Select(e => e.Type).Should().BeEquivalentTo(new[] { found1.Type, found2.Type });

            (await SearchService.Find(found1.Key)).Should().NotBeNull();
            (await SearchService.Find(found2.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMergeWhenAllDuplicates()
        {
            var targetKey = Guid.NewGuid();
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = item1.Text, Type = item1.Type };

            await SearchService.Merge(targetKey, item1);
            var merged = await SearchService.Merge(targetKey, new[] { item1, item2 });

            merged.Should().HaveCount(2);
            merged.Should().Contain(e => e.Key == item1.Key);
            merged.Should().NotContain(e => e.Key == item2.Key);

            (await SearchService.Find(item1.Key)).Should().NotBeNull();
            (await SearchService.Find(item2.Key)).Should().BeNull();
        }

        [Fact]
        public async Task CanFindTargets()
        {
            var targetKey1 = Guid.NewGuid();
            var targetKey2 = Guid.NewGuid();
            var targetKey3 = Guid.NewGuid();

            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item3 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item4 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };

            await SearchService.Merge(targetKey1, item1);
            await SearchService.Merge(targetKey2, new[] { item1, item2 });
            var foundItems = await SearchService.Merge(targetKey3, new[] { item1, item2, item3, item4 });

            var foundItem1 = foundItems.Single(e => e.Text == item1.Text);
            var foundItem2 = foundItems.Single(e => e.Text == item2.Text);
            var foundItem3 = foundItems.Single(e => e.Text == item3.Text);
            var foundItem4 = foundItems.Single(e => e.Text == item4.Text);

            var targets1 = await SearchService.FindTargets(foundItem1.Key);
            targets1.Should().HaveCount(3);
            targets1.Should().BeEquivalentTo(new[] { targetKey1, targetKey2, targetKey3 });

            var targets2 = await SearchService.FindTargets(foundItem2.Key);
            targets2.Should().HaveCount(2);
            targets2.Should().BeEquivalentTo(new[] { targetKey2, targetKey3 });

            var targets3 = await SearchService.FindTargets(foundItem3.Key);
            targets3.Should().HaveCount(1);
            targets3.Should().BeEquivalentTo(new[] { targetKey3 });

            var targets4 = await SearchService.FindTargets(foundItem4.Key);
            targets4.Should().HaveCount(1);
            targets4.Should().BeEquivalentTo(new[] { targetKey3 });
        }
    }
}
