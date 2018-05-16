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
            await SearchService.Merge(Guid.NewGuid(), new[] { item });
            SearchService.QuerySingle(item.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanMerge()
        {
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = "first", Type = SearchItemType.Organization };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = "second", Type = SearchItemType.Person };

            await SearchService.Merge(Guid.NewGuid(), new[] { item1, item2 });

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

            await SearchService.Merge(targetKey, item1);
            var merged = await SearchService.Merge(targetKey, new[] { item1, item2 });

            merged.Should().HaveCount(1);
            merged[0].Key.Should().Be(item2.Key);
            merged[0].Text.Should().Be(item2.Text);
            merged[0].Type.Should().Be(item2.Type);

            (await SearchService.Find(item1.Key)).Should().NotBeNull();
            (await SearchService.Find(item2.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMergeWithDuplicateWhenTypeDiffers()
        {
            var targetKey = Guid.NewGuid();
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = item1.Text, Type = SearchItemType.Organization };

            await SearchService.Merge(targetKey, item1);
            var merged = await SearchService.Merge(targetKey, new[] { item1, item2 });

            merged.Should().HaveCount(1);
            merged[0].Key.Should().Be(item2.Key);
            merged[0].Text.Should().Be(item2.Text);
            merged[0].Type.Should().Be(item2.Type);

            (await SearchService.Find(item1.Key)).Should().NotBeNull();
            (await SearchService.Find(item2.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMergeWhenAllDuplicates()
        {
            var targetKey = Guid.NewGuid();
            var item1 = new SearchItem { Key = Guid.NewGuid(), Text = Guid.NewGuid().ToString("N"), Type = SearchItemType.Person };
            var item2 = new SearchItem { Key = Guid.NewGuid(), Text = item1.Text, Type = item1.Type };

            await SearchService.Merge(targetKey, item1);
            var merged = await SearchService.Merge(targetKey, new[] { item1, item2 });

            merged.Should().HaveCount(0);

            (await SearchService.Find(item1.Key)).Should().NotBeNull();
            (await SearchService.Find(item2.Key)).Should().BeNull();
        }

        [Fact]
        public void CanFindTargets()
        {
            "TODO: Implement me!".Should().Be("");
        }
    }
}
