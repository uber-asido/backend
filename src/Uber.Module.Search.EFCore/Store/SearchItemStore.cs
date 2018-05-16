using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Store;

namespace Uber.Module.Search.EFCore.Store
{
    public class SearchItemStore : ISearchItemStore
    {
        private readonly DataStore db;

        public SearchItemStore(DataStore db)
        {
            this.db = db;
        }

        public IQueryable<SearchItem> Query() => db.SearchItems;
        public IQueryable<SearchItem> QuerySingle(Guid key) => db.SearchItems.Where(e => e.Key == key);

        public Task<SearchItem> Find(Guid key) => QuerySingle(key).SingleOrDefaultAsync();
        public Task<List<SearchItem>> Find(IEnumerable<string> texts, IEnumerable<SearchItemType> types) =>
            Query().Where(e => texts.Contains(e.Text) && types.Contains(e.Type)).ToListAsync();

        public Task<List<SearchItem>> FindFullText(string freeText)
        {
            freeText = freeText.ToLower();
            var result = (from item in db.SearchItems
                          where EF.Functions.ILike(item.Text, $"%{freeText}%")
                          select item).ToListAsync();
            return result;
        }

        public Task Insert(SearchItem search) => db.InsertAndCommit(search);
        public Task Insert(IEnumerable<SearchItem> searches) => db.InsertAndCommit(searches);
    }
}
