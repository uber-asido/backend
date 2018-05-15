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
            (await MovieService.Find(Guid.NewGuid())).Should().BeNull();

            var movie = new Abstraction.Model.Movie
            {
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new List<Actor>(),
                Directors = new List<Director>(),
                Distributors = new List<Distributor>(),
                FilmingLocations = new List<FilmingLocation>(),
                ProductionCompanies = new List<ProductionCompany>(),
                Writers = new List<Writer>()
            };
            var merged = await MovieService.Merge(movie);
            (await MovieService.Find(merged.Key)).Should().NotBeNull();
        }

        [Fact]
        public async Task CanMerge()
        {
            var movie = new Abstraction.Model.Movie
            {
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new[]
                {
                    new Actor { FullName = "Test actor 1" },
                    new Actor { FullName = "Test actor 2" }
                },
                Directors = new[]
                {
                    new Director { FullName = "Test director 1" },
                    new Director { FullName = "Test director 2" }
                },
                Distributors = new[]
                {
                    new Distributor { Name = "Test distributor 1" },
                    new Distributor { Name = "Test distributor 2" }
                },
                FilmingLocations = new[]
                {
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 1")).Key, FunFact = "Fun fact 1" },
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 2")).Key, FunFact = "Fun fact 2" }
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

            var merged = await MovieService.Merge(movie);
            var found = await MovieService.Find(merged.Key);

            foreach (var m in new[] { merged, found })
            {
                m.Should().NotBeNull();
                m.Title.Should().Be(movie.Title);
                m.ReleaseYear.Should().Be(movie.ReleaseYear);

                m.Actors.Should().HaveCount(2);
                m.Actors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Actors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test actor 1", "Test actor 2" });

                m.Directors.Should().HaveCount(2);
                m.Directors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Directors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test director 1", "Test director 2" });

                m.Distributors.Should().HaveCount(2);
                m.Distributors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Distributors.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test distributor 1", "Test distributor 2" });

                m.FilmingLocations.Should().HaveCount(2);
                m.FilmingLocations.Select(e => e.Key).Should().BeEquivalentTo(movie.FilmingLocations.Select(e => e.Key));
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

        [Fact]
        public async Task CanMergeWithExistingMovie()
        {
            var movie1 = new Abstraction.Model.Movie
            {
                Title = Guid.NewGuid().ToString(),
                ReleaseYear = 2018,
                Actors = new[]
                {
                    new Actor { FullName = "Test actor 1" },
                    new Actor { FullName = "Test actor 2" }
                },
                Directors = new[]
                {
                    new Director { FullName = "Test director 1" },
                    new Director { FullName = "Test director 2" }
                },
                Distributors = new[]
                {
                    new Distributor { Name = "Test distributor 1" },
                    new Distributor { Name = "Test distributor 2" }
                },
                FilmingLocations = new[]
                {
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 1")).Key, FunFact = "Fun fact 1" },
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 2")).Key, FunFact = "Fun fact 2" }
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

            var movie2 = new Abstraction.Model.Movie
            {
                Title = movie1.Title,
                ReleaseYear = movie1.ReleaseYear,
                Actors = new[]
                {
                    new Actor { FullName = "Test actor 2" },
                    new Actor { FullName = "Test actor 3" }
                },
                Directors = new[]
                {
                    new Director { FullName = "Test director 2" },
                    new Director { FullName = "Test director 3" }
                },
                Distributors = new[]
                {
                    new Distributor { Name = "Test distributor 2" },
                    new Distributor { Name = "Test distributor 3" }
                },
                FilmingLocations = new[]
                {
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 2")).Key, FunFact = "Fun fact 2" },
                    new FilmingLocation { Key = Guid.NewGuid(), AddressKey = (await GeocodingService.Geocode("Test address 3")).Key, FunFact = "Fun fact 3" }
                },
                ProductionCompanies = new[]
                {
                    new ProductionCompany { Name = "Test production company 2" },
                    new ProductionCompany { Name = "Test production company 3" }
                },
                Writers = new[]
                {
                    new Writer { FullName = "Test writer 2" },
                    new Writer { FullName = "Test writer 3" }
                }
            };

            var merged1 = await MovieService.Merge(movie1);
            var merged2 = await MovieService.Merge(movie2);
            var found = await MovieService.Find(merged1.Key);

            foreach (var m in new[] { merged2, found })
            {
                m.Should().NotBeNull();
                m.Key.Should().Be(merged1.Key);
                m.Title.Should().Be(movie1.Title);
                m.ReleaseYear.Should().Be(movie1.ReleaseYear);

                m.Actors.Should().HaveCount(3);
                m.Actors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Actors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test actor 1", "Test actor 2", "Test actor 3" });

                m.Directors.Should().HaveCount(3);
                m.Directors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Directors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test director 1", "Test director 2", "Test director 3" });

                m.Distributors.Should().HaveCount(3);
                m.Distributors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Distributors.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test distributor 1", "Test distributor 2", "Test distributor 3" });

                m.FilmingLocations.Should().HaveCount(3);
                m.FilmingLocations.Select(e => e.Key).Should().BeEquivalentTo(movie1.FilmingLocations.Select(e => e.Key).Concat(new[] { movie2.FilmingLocations[1].Key }));
                m.FilmingLocations.Select(e => e.AddressKey).Should().BeEquivalentTo(merged2.FilmingLocations.Select(e => e.AddressKey));
                m.FilmingLocations.Select(e => e.Latitude).Should().BeEquivalentTo(new[] { 1.0, 1.0, 1.0 });
                m.FilmingLocations.Select(e => e.Longitude).Should().BeEquivalentTo(new[] { 1.0, 1.0, 1.0 });
                m.FilmingLocations.Select(e => e.FormattedAddress).Should().BeEquivalentTo(new[] { "Test address 1", "Test address 2", "Test address 3" });
                m.FilmingLocations.Select(e => e.FunFact).Should().BeEquivalentTo(new[] { "Fun fact 1", "Fun fact 2", "Fun fact 3" });

                m.ProductionCompanies.Should().HaveCount(3);
                m.ProductionCompanies.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.ProductionCompanies.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test production company 1", "Test production company 2", "Test production company 3" });

                m.Writers.Should().HaveCount(3);
                m.Writers.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
                m.Writers.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test writer 1", "Test writer 2", "Test writer 3" });
            }
        }
    }
}
