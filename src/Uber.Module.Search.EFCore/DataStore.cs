using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<SearchItem> SearchItems => DataContext.SearchItems;

        public DataStore(DataContext context) : base(context) { }
    }
}
