using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Manager;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;

namespace Uber.Module.Search.Manager
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchItemStore searchStore;

        public SearchManager(ISearchItemStore searchStore)
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
