using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;
using Uber.Module.Search.Abstraction.Service;

namespace Uber.Module.Movie.Service
{
    internal class FilmingLocationService : IFilmingLocationService
    {
        private readonly IGeocodingService geocodingService;
        private readonly ISearchService searchService;
        private readonly IFilmingLocationStore locationStore;

        public FilmingLocationService(IGeocodingService geocodingService, ISearchService searchService, IFilmingLocationStore store)
        {
            this.geocodingService = geocodingService;
            this.searchService = searchService;
            this.locationStore = store;
        }

        public async Task<List<FilmingLocation>> Find()
        {
            var locations = await locationStore.Find();
            await ResolveLocations(locations);
            return locations;
        }

        public async Task<List<FilmingLocation>> Find(Guid searchItemKey)
        {
            var movieKeys = await searchService.FindTargets(searchItemKey);
            var locations = await MovieKeysToFilmingLocations(movieKeys);
            return locations;
        }

        public async Task<List<FilmingLocation>> Find(string freeText)
        {
            var movieKeys = await searchService.FindTargets(freeText);
            var locations = await MovieKeysToFilmingLocations(movieKeys);
            return locations;
        }

        public async Task ResolveLocations(IList<FilmingLocation> locations)
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

                    locations.Remove(location);
                    continue;
                }

                location.FormattedAddress = address.FormattedAddress;
                location.Latitude = address.Latitude;
                location.Longitude = address.Longitude;
            }
        }

        private async Task<List<FilmingLocation>> MovieKeysToFilmingLocations(IEnumerable<Guid> movieKeys)
        {
            var locations = await locationStore.Find(movieKeys);
            await ResolveLocations(locations);
            return locations;
        }
    }
}
