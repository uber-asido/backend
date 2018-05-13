using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Abstraction.Store
{
    public interface IGeocodingStore
    {
        IQueryable<Address> Query();
        IQueryable<Address> QuerySingle(Guid key);

        Task<Address> Find(string unformattedAddress);
        Task<Address> Find(Guid key);
        Task<List<Address>> Find(IEnumerable<Guid> keys);

        Task<Address> Create(string unformattedAddress, Address address);
    }
}
