using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;
using Uber.Module.Geocoding.EFCore.Entity;

namespace Uber.Module.Geocoding.EFCore.Store
{
    public class GeocodingStore : IGeocodingStore
    {
        private readonly DataStore db;

        public GeocodingStore(DataStore db)
        {
            this.db = db;
        }

        public IQueryable<Address> Query() => db.Addresses;
        public IQueryable<Address> QuerySingle(Guid key) => db.Addresses.Where(e => e.Key == key);

        public Task<Address> Find(Guid key) => QuerySingle(key).SingleOrDefaultAsync();
        public Task<List<Address>> Find(IEnumerable<Guid> keys) => Query().Where(e => keys.Contains(e.Key)).ToListAsync();

        public Task<Address> Find(string unformattedAddress)
        {
            var query = from location in db.Locations
                        join address in db.Addresses on location.AddressKey equals address.Key
                        where location.UnformattedAddress == unformattedAddress
                        select address;
            return query.FirstOrDefaultAsync();
        }

        public async Task<Address> Create(string unformattedAddress, Address address)
        {
            var entity = await db.Addresses.FirstOrDefaultAsync(e =>
                e.FormattedAddress == address.FormattedAddress &&
                e.Latitude == address.Latitude &&
                e.Longitude == address.Longitude
            );

            if (entity == null)
            {
                db.Insert(address);
                entity = address;
            }

            db.Insert(new Location { Key = Guid.NewGuid(), AddressKey = entity.Key, UnformattedAddress = unformattedAddress });
            await db.Commit();

            return entity;
        }
    }
}
