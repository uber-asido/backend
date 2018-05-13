using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Abstraction.Store
{
    public interface IGeocodingStore
    {
        IQueryable<Address> Query();
        IQueryable<Address> QuerySingle(Guid key);

        Task<Address> Create(Address address);
    }
}
