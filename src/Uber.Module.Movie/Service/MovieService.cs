using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.Service
{
    public class MovieService : IMovieService
    {
        private readonly IGeocodingService geocodingService;
        private readonly IMovieStore movieStore;

        public MovieService(IGeocodingService geocodingService, IMovieStore movieStore)
        {
            this.geocodingService = geocodingService;
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
            if (movie.Key == default(Guid))
                movie.Key = Guid.NewGuid();

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

            foreach (var location in movie.FilmingLocations)
            {
                var address = addresses.Single(e => e.Key == location.AddressKey);
                location.FormattedAddress = address.FormattedAddress;
                location.Latitude = address.Latitude;
                location.Longitude = address.Longitude;
            }
        }
    }
}
