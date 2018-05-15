using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Service;

namespace Uber.Core.Test.Mock
{
    public class SearchServiceMock : ISearchService
    {
        private readonly List<SearchItem> store = new List<SearchItem>();

        public IQueryable<SearchItem> Query()
        {
            lock (this)
                return store.ToList().AsQueryable();
        }

        public IQueryable<SearchItem> QuerySingle(Guid key)
        {
            lock (this)
                return store.ToList().Where(e => e.Key == key).AsQueryable();
        }

        public Task<SearchItem> Find(Guid key)
        {
            lock (this)
                return Task.FromResult(store.ToList().SingleOrDefault(e => e.Key == key));
        }

        public Task<SearchItem> Create(SearchItem search)
        {
            if (search.Key == default(Guid))
                search.Key = Guid.NewGuid();

            store.Add(search);

            return Task.FromResult(search);
        }
        
        public Task<List<SearchItem>> Merge(IEnumerable<SearchItem> items)
        {
            var inserted = new List<SearchItem>();

            lock (this)
            {
                foreach (var item in items)
                {
                    if (!store.Any(e => e.Text == item.Text && e.Type == item.Type))
                    {
                        if (item.Key == default(Guid))
                            item.Key = Guid.NewGuid();

                        store.Add(item);
                        inserted.Add(item);
                    }
                }
            }

            return Task.FromResult(inserted);
        }
    }
}
