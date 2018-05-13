using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Service;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;

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

        public async Task<SearchItem> Create(SearchItem search)
        {
            if (search.Key == default(Guid))
                search.Key = Guid.NewGuid();

            await searchStore.Create(search);
            return search;
        }
    }
}
