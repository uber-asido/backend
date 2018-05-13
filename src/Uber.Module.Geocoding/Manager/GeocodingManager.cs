using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Manager;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Manager
{
    public class GeocodingManager : IGeocodingManager
    {
        public IQueryable<Address> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Address> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<Address> Resolve(string address)
        {
            throw new NotImplementedException();
        }
    }
}
