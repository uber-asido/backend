using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;

namespace Uber.Module.Geocoding.Service
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IGeocodeProvider geocodeProvider;
        private readonly IGeocodingStore geocodeStore;

        public GeocodingService(IGeocodeProvider geocodeProvider, IGeocodingStore geocodeStore)
        {
            this.geocodeProvider = geocodeProvider;
            this.geocodeStore = geocodeStore;
        }

        public IQueryable<Address> Query() => geocodeStore.Query();
        public IQueryable<Address> QuerySingle(Guid key) => geocodeStore.QuerySingle(key);

        public async Task<Address> Resolve(string unformattedAddress)
        {
            var address = await geocodeStore.Find(unformattedAddress);

            if (address == null)
            {
                var geocode = await geocodeProvider.Geocode(unformattedAddress);
                address = await geocodeStore.Create(unformattedAddress, new Address
                {
                    Key = Guid.NewGuid(),
                    FormattedAddress = geocode.FormattedAddress,
                    Latitude = geocode.Latitude,
                    Longitude = geocode.Longitude
                });
            }

            return address;
        }
    }
}
