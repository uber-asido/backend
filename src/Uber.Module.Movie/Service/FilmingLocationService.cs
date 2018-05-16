using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.Service
{
    internal class FilmingLocationService : IFilmingLocationService
    {
        private readonly IGeocodingService geocodingService;
        private readonly IFilmingLocationStore store;

        public FilmingLocationService(IGeocodingService geocodingService, IFilmingLocationStore store)
        {
            this.geocodingService = geocodingService;
            this.store = store;
        }

        public async Task<List<FilmingLocation>> Find()
        {
            var locations = await store.Find();
            await ResolveLocations(locations);

            // Remove what couldn't be resolved.
            locations = locations
                .Where(e => e.FormattedAddress != null)
                .ToList();

            return locations;
        }

        public async Task ResolveLocations(IEnumerable<FilmingLocation> locations)
        {
            if (!locations.Any())
                return;

            var keys = locations.Select(e => e.AddressKey);
            var addresses = await geocodingService.Find(keys);

            foreach (var location in locations.ToList())
            {
                var address = addresses.SingleOrDefault(e => e.Key == location.AddressKey);
                if (address == null)
                {
                    // Geocode service doesn't have such address - ignore.
                    // TODO: Figure what to do with them:
                    //  o Resolve geocode from unformatted address again?
                    //  o Remove the location from the movie?

                    continue;
                }

                location.FormattedAddress = address.FormattedAddress;
                location.Latitude = address.Latitude;
                location.Longitude = address.Longitude;
            }
        }
    }
}
