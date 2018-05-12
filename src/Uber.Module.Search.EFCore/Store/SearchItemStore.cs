using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;

namespace Uber.Module.Search.EFCore.Store
{
    public class SearchItemStore : ISearchItemStore
    {
        private readonly DataStore db;

        public SearchItemStore(DataStore db)
        {
            this.db = db;
        }

        public IQueryable<SearchItem> Query() => db.SearchItems;
        public IQueryable<SearchItem> QuerySingle(Guid key) => db.SearchItems.Where(e => e.Key == key);

        public Task Create(SearchItem search) => db.InsertAndCommit(search);
    }
}
