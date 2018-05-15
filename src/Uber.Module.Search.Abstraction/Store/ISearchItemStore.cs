using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Abstraction.Store
{
    public interface ISearchItemStore
    {
        IQueryable<SearchItem> Query();
        IQueryable<SearchItem> QuerySingle(Guid key);

        Task<SearchItem> Find(Guid key);
        Task<List<SearchItem>> Find(IEnumerable<string> texts, IEnumerable<SearchItemType> types);

        Task Insert(SearchItem search);
        Task Insert(IEnumerable<SearchItem> searches);
    }
}
