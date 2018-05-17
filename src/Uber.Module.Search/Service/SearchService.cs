using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Service;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;
using System.Collections.Generic;

namespace Uber.Module.Search.Service
{
    public class SearchService : ISearchService
    {
        private readonly ISearchItemStore searchStore;
        private readonly ISearchItemTargetStore targetStore;

        public SearchService(ISearchItemStore searchStore, ISearchItemTargetStore targetStore)
        {
            this.searchStore = searchStore;
            this.targetStore = targetStore;
        }

        public IQueryable<SearchItem> Query() => searchStore.Query();
        public IQueryable<SearchItem> QuerySingle(Guid key) => searchStore.QuerySingle(key);

        public Task<SearchItem> Find(Guid key) => searchStore.Find(key);

        public async Task<IEnumerable<Guid>> FindTargets(Guid searchItemKey)
        {
            var targets = await targetStore.Find(searchItemKey);
            return targets.Select(e => e.TargetKey);
        }

        public async Task<IEnumerable<Guid>> FindTargets(string freeText)
        {
            var searchItems = await searchStore.FindFullText(freeText);
            if (!searchItems.Any())
                return new List<Guid>();

            var targets = await targetStore.Find(searchItems.Select(e => e.Key));
            return targets.Select(e => e.TargetKey);
        }

        public async Task<SearchItem> Merge(Guid targetKey, SearchItem search)
        {
            var result = await Merge(targetKey, new[] { search });
            return result.Single();
        }

        public async Task<List<SearchItem>> Merge(Guid targetKey, IEnumerable<SearchItem> searches)
        {
            var texts = searches.Select(e => e.Text).Distinct();
            var types = searches.Select(e => e.Type).Distinct();

            var existingSearch = await searchStore.Find(texts, types);
            var existingTargets = await targetStore.Find(existingSearch.Select(e => e.Key));
            var toInsertSearch = new List<SearchItem>();
            var toInsertTargets = new List<SearchItemTarget>();
            var results = new List<SearchItem>();

            foreach (var item in searches)
            {
                var existing = existingSearch.FirstOrDefault(e => e.Text == item.Text && e.Type == item.Type);
                if (existing == null)
                {
                    if (item.Key == default(Guid))
                        item.Key = Guid.NewGuid();

                    toInsertSearch.Add(item);
                    existingSearch.Add(item);

                    var target = new SearchItemTarget { SearchItemKey = item.Key, TargetKey = targetKey };
                    toInsertTargets.Add(target);
                    existingTargets.Add(target);

                    results.Add(item);
                }
                else
                {
                    if (!existingTargets.Any(e => e.SearchItemKey == existing.Key && e.TargetKey == targetKey))
                    {
                        var target = new SearchItemTarget { SearchItemKey = existing.Key, TargetKey = targetKey };
                        toInsertTargets.Add(target);
                        existingTargets.Add(target);
                    }

                    results.Add(existing);
                }
            }

            await targetStore.Insert(toInsertTargets);
            await searchStore.Insert(toInsertSearch);

            return results;
        }
    }
}
