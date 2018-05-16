using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Module.Movie.Test
{
    public class MovieTestBase : IDisposable
    {
        public readonly IFilmingLocationService FilmingLocationService;
        public readonly IMovieService MovieService;
        public readonly IGeocodingService GeocodingService;

        private readonly IServiceScope scope;

        public MovieTestBase(MovieFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            FilmingLocationService = scope.ServiceProvider.GetRequiredService<IFilmingLocationService>();
            MovieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
            GeocodingService = scope.ServiceProvider.GetRequiredService<IGeocodingService>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
