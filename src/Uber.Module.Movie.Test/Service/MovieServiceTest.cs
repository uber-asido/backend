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
    public class MovieServiceTest : MovieTestBase
    {
        public MovieServiceTest(MovieFixture fixture) : base(fixture) { }

        [Fact]
        public async Task CanFind()
        {
            (await MovieSerice.Find(Guid.NewGuid())).Should().BeNull();

            var movie = new Abstraction.Model.Movie
            {
                Key = Guid.NewGuid(),
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new List<Actor>(),
                Distributors = new List<Distributor>(),
                FilmingLocations = new List<FilmingLocation>(),
                ProductionCompanies = new List<ProductionCompany>(),
                Writers = new List<Writer>()
            };
            await MovieSerice.Merge(movie);
            (await MovieSerice.Find(movie.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMerge()
        {
            var movie = new Abstraction.Model.Movie
            {
                Key = Guid.NewGuid(),
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new[]
                {
                    new Actor { FullName = "Test actor 1" },
                    new Actor { FullName = "Test actor 2" }
                },
                Distributors = new[]
                {
                    new Distributor { Name = "Test distributor 1" },
                    new Distributor { Name = "Test distributor 2" }
                },
                FilmingLocations = new[]
                {
                    new FilmingLocation { AddressKey = (await GeocodingService.Geocode("Test address 1")).Key, FunFact = "Fun fact 1" },
                    new FilmingLocation { AddressKey = (await GeocodingService.Geocode("Test address 2")).Key, FunFact = "Fun fact 2" }
                },
                ProductionCompanies = new[]
                {
                    new ProductionCompany { Name = "Test production company 1" },
                    new ProductionCompany { Name = "Test production company 2" }
                },
                Writers = new[]
                {
                    new Writer { FullName = "Test writer 1" },
                    new Writer { FullName = "Test writer 2" }
                }
            };

            var merged = await MovieSerice.Merge(movie);
            var found = await MovieSerice.Find(movie.Key);

            foreach (var m in new[] { merged, found })
            {
                m.Should().NotBeNull();
                m.Title.Should().Be(movie.Title);
                m.ReleaseYear.Should().Be(movie.ReleaseYear);

                m.Actors.Should().HaveCount(2);
                m.Actors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Actors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test actor 1", "Test actor 2" });

                m.Distributors.Should().HaveCount(2);
                m.Distributors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Distributors.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test distributor 1", "Test distributor 2" });

                m.FilmingLocations.Should().HaveCount(2);
                m.FilmingLocations.Select(e => e.AddressKey).Should().BeEquivalentTo(movie.FilmingLocations.Select(e => e.AddressKey));
                m.FilmingLocations.Select(e => e.Latitude).Should().BeEquivalentTo(new[] { 1.0, 1.0 });
                m.FilmingLocations.Select(e => e.Longitude).Should().BeEquivalentTo(new[] { 1.0, 1.0 });
                m.FilmingLocations.Select(e => e.FormattedAddress).Should().BeEquivalentTo(new[] { "Test address 1", "Test address 2" });
                m.FilmingLocations.Select(e => e.FunFact).Should().BeEquivalentTo(new[] { "Fun fact 1", "Fun fact 2" });

                m.ProductionCompanies.Should().HaveCount(2);
                m.ProductionCompanies.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.ProductionCompanies.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test production company 1", "Test production company 2" });

                m.Writers.Should().HaveCount(2);
                m.Writers.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Writers.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test writer 1", "Test writer 2" });
            }
        }
    }
}
