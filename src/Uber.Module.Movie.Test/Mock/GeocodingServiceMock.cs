using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Service;

namespace Uber.Module.Movie.Test.Mock
{
    public class GeocodingServiceMock : IGeocodingService
    {
        private readonly List<Address> store = new List<Address>();

        public IQueryable<Address> Query() => store.AsQueryable();
        public IQueryable<Address> QuerySingle(Guid key) => store.Where(e => e.Key == key).AsQueryable();

        public Task<Address> Find(Guid key) => Task.FromResult(store.SingleOrDefault(e => e.Key == key));
        public Task<IList<Address>> Find(IEnumerable<Guid> keys) => Task.FromResult<IList<Address>>(store.Where(e => keys.Contains(e.Key)).ToList());

        public Task<Address> Geocode(string location)
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
