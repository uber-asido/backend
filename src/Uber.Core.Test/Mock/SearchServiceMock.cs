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
        private readonly List<SearchItem> searchStore = new List<SearchItem>();
        private readonly Dictionary<SearchItem, List<Guid>> targetStore = new Dictionary<SearchItem, List<Guid>>();

        public IQueryable<SearchItem> Query()
        {
            lock (this)
                return searchStore.ToList().AsQueryable();
        }

        public IQueryable<SearchItem> QuerySingle(Guid key)
        {
            lock (this)
                return searchStore.ToList().Where(e => e.Key == key).AsQueryable();
        }

        public Task<SearchItem> Find(Guid key)
        {
            lock (this)
                return Task.FromResult(searchStore.ToList().SingleOrDefault(e => e.Key == key));
        }

        public Task<List<Guid>> FindTargets(Guid searchItemKey)
        {
            lock (this)
            {
                var searchItem = searchStore.SingleOrDefault(e => e.Key == searchItemKey);
                if (searchItem == null)
                    return Task.FromResult(new List<Guid>());

                if (targetStore.TryGetValue(searchItem, out var targets))
                    return Task.FromResult(targets);
            }

            return Task.FromResult(new List<Guid>());
        }

        public Task<List<Guid>> FindTargets(string freeText)
        {
            freeText = freeText.ToLower();

            lock (this)
            {
                var searchItems = searchStore.Where(e => e.Text.ToLower().Contains(freeText));
                if (!searchItems.Any())
                    return Task.FromResult(new List<Guid>());

                var targets = new List<Guid>();
                foreach (var item in searchItems)
                {
                    if (targetStore.TryGetValue(item, out var t))
                        targets.AddRange(t);
                }
                targets = targets.Distinct().ToList();
                return Task.FromResult(targets);
            }
        }

        public async Task<SearchItem> Merge(Guid targetKey, SearchItem item)
        {
            var result = await Merge(targetKey, new[] { item });
            return result.SingleOrDefault();
        }

        public Task<List<SearchItem>> Merge(Guid targetKey, IEnumerable<SearchItem> items)
        {
            var inserted = new List<SearchItem>();

            lock (this)
            {
                foreach (var item in items)
                {
                    var existingItem = searchStore.SingleOrDefault(e => e.Text == item.Text && e.Type == item.Type);

                    if (existingItem == null)
                    {
                        if (item.Key == default(Guid))
                            item.Key = Guid.NewGuid();

                        searchStore.Add(item);
                        targetStore.Add(item, new List<Guid> { targetKey });

                        inserted.Add(item);
                    }
                    else
                    {
                        if (targetStore.TryGetValue(existingItem, out var targets))
                        {
                            if (!targets.Contains(targetKey))
                                targets.Add(targetKey);
                        }
                        else
                        {
                            throw new InvalidOperationException("Search item present, but target is not.");
                        }
                    }
                }
            }

            return Task.FromResult(inserted);
        }
    }
}
