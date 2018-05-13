using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<Address> Addresses => DataContext.Addresses;

        public DataStore(DataContext context) : base(context) { }
    }
}
