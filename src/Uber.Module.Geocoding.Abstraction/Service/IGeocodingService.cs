using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Abstraction.Service
{
    public interface IGeocodingService
    {
        IQueryable<Address> Query();
        IQueryable<Address> QuerySingle(Guid key);

        Task<Address> Find(Guid key);
        Task<IList<Address>> Find(IEnumerable<Guid> keys);

        Task<Address> Geocode(string location);
    }
}
