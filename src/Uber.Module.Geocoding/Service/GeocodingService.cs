﻿using System;
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

        public GeocodingService(IGeocodeProvider geocodeProvider, IGeocodingStore geocodeStore)
        {
            this.geocodeProvider = geocodeProvider;
            this.geocodeStore = geocodeStore;
        }

        public IQueryable<Address> Query() => geocodeStore.Query();
        public IQueryable<Address> QuerySingle(Guid key) => geocodeStore.QuerySingle(key);

        public Task<Address> Find(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Address>> Find(IEnumerable<Guid> keys)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> Geocode(string location)
        {
            var address = await geocodeStore.Find(location);

            if (address == null)
            {
                var geocode = await geocodeProvider.Geocode(location);
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
