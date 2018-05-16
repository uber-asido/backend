using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;
using Xunit;

namespace Uber.Module.Movie.Test.Service
{
    [Collection(MovieTestCollection.Name)]
    public class FilmingLocationServiceTest : MovieTestBase
    {
        public FilmingLocationServiceTest(MovieFixture fixture) : base(fixture) { }

        [Fact]
        public async Task CanFindLocations()
        {
            (await FilmingLocationService.Find()).Should().NotBeNull();
        }

        [Fact]
        public async Task CanFindSpecificMovieLocations()
        {
            var movie = new Abstraction.Model.Movie
            {
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new List<Actor>(),
                Directors = new List<Director>(),
                Distributors = new List<Distributor>(),
                FilmingLocations = new[]
                {
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 1")).Key, FunFact = "Fun fact 1" },
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 2")).Key, FunFact = "Fun fact 2" }
                },
                ProductionCompanies = new List<ProductionCompany>(),
                Writers = new List<Writer>()
            };

            var merged = await MovieService.Merge(movie);

            var locations = await FilmingLocationService.Find();
            var movieLocations = locations.Where(e => e.MovieKey == merged.Key).ToList();
            movieLocations.Should().HaveCount(2);
            movieLocations.Select(e => e.Key).Should().BeEquivalentTo(movie.FilmingLocations.Select(e => e.Key));
            movieLocations.Select(e => e.MovieKey).All(key => key != default(Guid)).Should().BeTrue();
            movieLocations.Select(e => e.MovieKey).Should().BeEquivalentTo(merged.FilmingLocations.Select(e => e.MovieKey));
            movieLocations.Select(e => e.AddressKey).Should().BeEquivalentTo(movie.FilmingLocations.Select(e => e.AddressKey));
            movieLocations.Select(e => e.Latitude).Should().BeEquivalentTo(new[] { 1.0, 1.0 });
            movieLocations.Select(e => e.Longitude).Should().BeEquivalentTo(new[] { 1.0, 1.0 });
            movieLocations.Select(e => e.FormattedAddress).Should().BeEquivalentTo(new[] { "Test address 1", "Test address 2" });
            movieLocations.Select(e => e.FunFact).Should().BeEquivalentTo(new[] { "Fun fact 1", "Fun fact 2" });
        }
    }
}
