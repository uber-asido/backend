using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Search.Abstraction.Model;

namespace Uber.Module.Movie.Search.Abstraction.Manager
{
    public interface ISearchManager
    {
        IQueryable<SearchItem> Query();
        IQueryable<SearchItem> QuerySingle(Guid key);

        Task<SearchItem> Create(SearchItem search);
    }
}
