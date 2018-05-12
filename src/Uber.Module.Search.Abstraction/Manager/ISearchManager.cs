using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Abstraction.Manager
{
    public interface ISearchManager
    {
        IQueryable<SearchItem> Query();
        IQueryable<SearchItem> QuerySingle(Guid key);

        Task<SearchItem> Create(SearchItem search);
    }
}
