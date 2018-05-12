using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Abstraction.Store
{
    public interface ISearchItemStore
    {
        IQueryable<SearchItem> Query();
        IQueryable<SearchItem> QuerySingle(Guid key);

        Task Create(SearchItem search);
    }
}
