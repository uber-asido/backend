using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Search.Abstraction.Model;
using Uber.Module.Movie.Search.Abstraction.Store;

namespace Uber.Module.Movie.Search.EFCore.Store
{
    public class SearchItemStore : ISearchItemStore
    {
        public IQueryable<SearchItem> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<SearchItem> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<SearchItem> Create(SearchItem search)
        {
            throw new NotImplementedException();
        }
    }
}
