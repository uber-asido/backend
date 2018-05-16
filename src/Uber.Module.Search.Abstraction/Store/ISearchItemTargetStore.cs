using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uber.Module.Search.Abstraction.Store
{
    public interface ISearchItemTargetStore
    {
        Task<List<Guid>> Find(Guid searchItemKey);
        Task<List<Guid>> Find(IEnumerable<Guid> searchItemKeys);

        Task Insert(Guid targetKey, IEnumerable<Guid> searchItemKeys);
    }
}
