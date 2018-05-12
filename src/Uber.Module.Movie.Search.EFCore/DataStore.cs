using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;
using Uber.Module.Movie.Search.Abstraction.Model;

namespace Uber.Module.Movie.Search.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<SearchItem> SearchItems => DataContext.SearchItems;

        public DataStore(DataContext context) : base(context) { }
    }
}
