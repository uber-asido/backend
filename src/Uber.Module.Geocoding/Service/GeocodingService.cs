using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Service
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IGeocodeProvider geocodeProvider;

        public GeocodingService(IGeocodeProvider geocodeProvider)
        {
            this.geocodeProvider = geocodeProvider;
        }

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
