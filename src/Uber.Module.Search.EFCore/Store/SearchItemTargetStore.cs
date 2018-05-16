using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;

namespace Uber.Module.Search.EFCore.Store
{
    public class SearchItemTargetStore : ISearchItemTargetStore
    {
        private readonly DataStore db;

        public SearchItemTargetStore(DataStore db)
        {
            this.db = db;
        }

        public Task<List<Guid>> Find(Guid searchItemKey) =>
            db.SearchItemTargets
            .Where(e => e.SearchItemKey == searchItemKey)
            .Select(e => e.TargetKey)
            .ToListAsync();

        public Task<List<Guid>> Find(IEnumerable<Guid> searchItemKeys) =>
            db.SearchItemTargets
            .Where(e => searchItemKeys.Contains(e.SearchItemKey))
            .Select(e => e.TargetKey)
            .ToListAsync();

        public async Task Insert(Guid targetKey, IEnumerable<Guid> searchItemKeys)
        {
            var entities = searchItemKeys.Select(key => new SearchItemTarget { SearchItemKey = key, TargetKey = targetKey });
            await db.InsertAndCommit(entities);
        }
    }
}
