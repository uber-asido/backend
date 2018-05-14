using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.EFCore.Entity;

namespace Uber.Module.Geocoding.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<Address> Addresses => DataContext.Addresses;
        public DbSet<Location> Locations => DataContext.Locations;

        public DataStore(DataContext context) : base(context) { }
    }
}
