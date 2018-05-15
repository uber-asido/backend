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

        public SearchService(ISearchItemStore searchStore)
        {
            this.searchStore = searchStore;
        }

        public IQueryable<SearchItem> Query() => searchStore.Query();
        public IQueryable<SearchItem> QuerySingle(Guid key) => searchStore.QuerySingle(key);

        public Task<SearchItem> Find(Guid key) => searchStore.Find(key);

        public async Task<SearchItem> Create(SearchItem search)
        {
            if (search.Key == default(Guid))
                search.Key = Guid.NewGuid();

            await searchStore.Insert(search);
            return search;
        }

        public async Task<List<SearchItem>> Merge(IEnumerable<SearchItem> searches)
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
                }
            }

            await searchStore.Insert(toInsertSearch);

            return toInsertSearch;
        }
    }
}
