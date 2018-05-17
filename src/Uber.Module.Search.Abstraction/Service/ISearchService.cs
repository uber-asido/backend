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

        Task<IEnumerable<Guid>> FindTargets(Guid searchItemKey);
        Task<IEnumerable<Guid>> FindTargets(string freeText);

        Task<SearchItem> Merge(Guid targetKey, SearchItem items);
        Task<List<SearchItem>> Merge(Guid targetKey, IEnumerable<SearchItem> items);
    }
}
