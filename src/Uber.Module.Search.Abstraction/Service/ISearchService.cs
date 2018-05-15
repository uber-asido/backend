using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Abstraction.Service
{
    public interface ISearchService
    {
        IQueryable<SearchItem> Query();
        IQueryable<SearchItem> QuerySingle(Guid key);

        Task<SearchItem> Find(Guid key);

        Task<SearchItem> Create(SearchItem search);
        Task<List<SearchItem>> Merge(IEnumerable<SearchItem> items);
    }
}
