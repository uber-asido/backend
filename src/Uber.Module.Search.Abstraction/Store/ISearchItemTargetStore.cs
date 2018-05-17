using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Abstraction.Store
{
    public interface ISearchItemTargetStore
    {
        Task<List<SearchItemTarget>> Find(Guid searchItemKey);
        Task<List<SearchItemTarget>> Find(IEnumerable<Guid> searchItemKeys);

        Task Insert(IEnumerable<SearchItemTarget> searchItem);
    }
}
