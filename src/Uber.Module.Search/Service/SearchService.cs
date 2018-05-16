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

        public Task<List<Guid>> FindTargets(Guid searchItemKey) => targetStore.Find(searchItemKey);

        public async Task<List<Guid>> FindTargets(string freeText)
        {
            var searchItems = await searchStore.FindFullText(freeText);
            if (!searchItems.Any())
                return new List<Guid>();

            var targets = await targetStore.Find(searchItems.Select(e => e.Key));
            return targets;
        }

        public async Task<SearchItem> Merge(Guid targetKey, SearchItem search)
        {
            var result = await Merge(targetKey, new[] { search });
            return result.SingleOrDefault();
        }

        public async Task<List<SearchItem>> Merge(Guid targetKey, IEnumerable<SearchItem> searches)
        {
            var texts = searches.Select(e => e.Text).Distinct();
            var types = searches.Select(e => e.Type).Distinct();

            var existingSearch = await searchStore.Find(texts, types);
            var toInsertSearch = new List<SearchItem>();

            foreach (var item in searches)
            {
                if (!existingSearch.Any(e => e.Text == item.Text && e.Type == item.Type))
                {
                    if (item.Key == default(Guid))
                        item.Key = Guid.NewGuid();

                    toInsertSearch.Add(item);
                    existingSearch.Add(item);
                }
            }

            // TODO: Populate search_item_target table.
            await searchStore.Insert(toInsertSearch);

            return toInsertSearch;
        }
    }
}
