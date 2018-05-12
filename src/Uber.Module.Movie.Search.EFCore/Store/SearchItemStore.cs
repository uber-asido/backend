using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Search.Abstraction.Model;
using Uber.Module.Movie.Search.Abstraction.Store;

namespace Uber.Module.Movie.Search.EFCore.Store
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
