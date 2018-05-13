using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Abstraction.Manager
{
    public interface IGeocodingManager
    {
        IQueryable<Address> Query();
        IQueryable<Address> QuerySingle(Guid key);

        Task<Address> Resolve(string address);
    }
}
