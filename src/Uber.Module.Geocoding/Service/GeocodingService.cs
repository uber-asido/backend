using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;
using System.Collections.Generic;

namespace Uber.Module.Geocoding.Service
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IGeocodeProvider geocodeProvider;
        private readonly IGeocodingStore geocodeStore;
        private readonly INotFoundStore notFoundStore;

        public GeocodingService(IGeocodeProvider geocodeProvider, IGeocodingStore geocodeStore, INotFoundStore notFoundStore)
        {
            this.geocodeProvider = geocodeProvider;
            this.geocodeStore = geocodeStore;
            this.notFoundStore = notFoundStore;
        }

        public IQueryable<Address> Query() => geocodeStore.Query();
        public IQueryable<Address> QuerySingle(Guid key) => geocodeStore.QuerySingle(key);

        public Task<Address> Find(Guid key) => geocodeStore.Find(key);
        public Task<List<Address>> Find(IEnumerable<Guid> keys) => geocodeStore.Find(keys);

        public async Task<Address> Geocode(string location)
        {
            var address = await geocodeStore.Find(location);

            if (address == null)
            {
                if (await notFoundStore.Find(location) != null)
                    return null;

                var geocode = await geocodeProvider.Geocode(location);
                if (geocode == null)
                {
                    await notFoundStore.Insert(new LocationNotFound { Key = Guid.NewGuid(), UnformattedAddress = location });
                    return null;
                }

                address = await geocodeStore.Create(location, new Address
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
