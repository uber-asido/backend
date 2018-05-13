using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;

namespace Uber.Module.Geocoding.EFCore.Store
{
    public class GeocodingStore : IGeocodingStore
    {
        public IQueryable<Address> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Address> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<Address> Create(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
