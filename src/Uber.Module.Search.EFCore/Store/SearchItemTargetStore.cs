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

        public Task<List<SearchItemTarget>> Find(Guid searchItemKey) =>
            db.SearchItemTargets
            .Where(e => e.SearchItemKey == searchItemKey)
            .ToListAsync();

        public Task<List<SearchItemTarget>> Find(IEnumerable<Guid> searchItemKeys) =>
            db.SearchItemTargets
            .Where(e => searchItemKeys.Contains(e.SearchItemKey))
            .ToListAsync();

        public Task Insert(IEnumerable<SearchItemTarget> targets) => db.InsertAndCommit(targets);
    }
}
