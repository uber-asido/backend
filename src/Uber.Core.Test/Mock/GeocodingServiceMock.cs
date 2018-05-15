using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Service;

namespace Uber.Core.Test.Mock
{
    public class GeocodingServiceMock : IGeocodingService
    {
        private readonly List<Address> store = new List<Address>();

        public IQueryable<Address> Query()
        {
            lock (this)
                return store.ToList().AsQueryable();
        }

        public IQueryable<Address> QuerySingle(Guid key)
        {
            lock (this)
                return store.ToList().Where(e => e.Key == key).AsQueryable();
        }

        public Task<Address> Find(Guid key)
        {
            lock (this)
                return Task.FromResult(store.ToList().SingleOrDefault(e => e.Key == key));
        }

        public Task<List<Address>> Find(IEnumerable<Guid> keys)
        {
            lock (this)
                return Task.FromResult(store.ToList().Where(e => keys.Contains(e.Key)).ToList());
        }

        public Task<Address> Geocode(string location)
        {
            lock (this)
            {
                var address = store.SingleOrDefault(e => e.FormattedAddress == location);

                if (address == null)
                {
                    address = new Address
                    {
                        Key = Guid.NewGuid(),
                        Latitude = 1,
                        Longitude = 1,
                        FormattedAddress = location
                    };

                    store.Add(address);
                }

                return Task.FromResult(address);
            }
        }
    }
}
