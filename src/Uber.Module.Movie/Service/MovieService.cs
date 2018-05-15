using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;
using Uber.Module.Search.Abstraction.Service;

namespace Uber.Module.Movie.Service
{
    public class MovieService : IMovieService
    {
        private readonly IGeocodingService geocodingService;
        private readonly ISearchService searchService;
        private readonly IMovieStore movieStore;

        public MovieService(IGeocodingService geocodingService, ISearchService searchService, IMovieStore movieStore)
        {
            this.geocodingService = geocodingService;
            this.searchService = searchService;
            this.movieStore = movieStore;
        }

        public async Task<Abstraction.Model.Movie> Find(Guid key)
        {
            var movie = await movieStore.Find(key);
            if (movie != null)
                await ResolveLocations(movie);
            return movie;
        }

        public async Task<Abstraction.Model.Movie> Merge(Abstraction.Model.Movie movie)
        {
            var movieNew = await movieStore.Merge(movie);
            await ResolveLocations(movieNew);

            return movieNew;
        }

        private async Task ResolveLocations(Abstraction.Model.Movie movie)
        {
            if (!movie.FilmingLocations.Any())
                return;

            var keys = movie.FilmingLocations.Select(e => e.AddressKey);
            var addresses = await geocodingService.Find(keys);

            foreach (var location in movie.FilmingLocations.ToList())
            {
                var address = addresses.SingleOrDefault(e => e.Key == location.AddressKey);
                if (address == null)
                {
                    // Geocode service doesn't have such address - ignore.
                    // TODO: Figure what to do with them:
                    //  o Resolve geocode from unformatted address again?
                    //  o Remove the location from the movie?

                    movie.FilmingLocations.Remove(location);
                    continue;
                }

                location.FormattedAddress = address.FormattedAddress;
                location.Latitude = address.Latitude;
                location.Longitude = address.Longitude;
            }
        }
    }
}
