using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Model;
using Xunit;

namespace Uber.Module.Movie.Test.Manager
{
    [Collection(MovieTestCollection.Name)]
    public class MovieManagerTest : MovieTestBase
    {
        public MovieManagerTest(MovieFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            MovieManager.Query().Should().NotBeNull();
        }

        [Fact]
        public async Task CanQuerySingle()
        {
            MovieManager.QuerySingle(Guid.NewGuid()).Should().BeEmpty();

            var movie = new Abstraction.Model.Movie { Key = Guid.NewGuid(), Title = "", ReleaseYear = 2018 };
            var found = await MovieManager.Create(movie);
            MovieManager.QuerySingle(movie.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanCreate()
        {
            var movie = new Abstraction.Model.Movie
            {
                Key = Guid.NewGuid(),
                Title = "Test movie",
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
                FilmingAddresses = new[]
                {
                    // TODO: Mock geocoding service instead.
                    new Address { Key = Guid.NewGuid(), FormattedAddress = "Test address 1", Latitude = 1, Longitude = 1 },
                    new Address { Key = Guid.NewGuid(), FormattedAddress = "Test address 2", Latitude = 2, Longitude = 2 }
                },
                FunFacts = new[]
                {
                    "Test fun fact 1",
                    "Test fun fact 2"
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
            await MovieManager.Create(movie);

            var found = MovieManager.QuerySingle(movie.Key).SingleOrDefault();
            found.Should().NotBeNull();
            found.Title.Should().Be(movie.Title);
            found.ReleaseYear.Should().Be(movie.ReleaseYear);

            found.Actors.Should().HaveCount(2);
            found.Actors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
            found.Actors.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test actor 1", "Test actor 2" });

            found.Distributors.Should().HaveCount(2);
            found.Distributors.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
            found.Distributors.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test distributor 1", "Test distributor 2" });

            // TODO: Implement once geocoding service is mocked.
            // found.FilmingAddresses...

            found.FunFacts.Should().HaveCount(2);
            found.FunFacts.Should().BeEquivalentTo(new[] { "Test fun fact 1", "Test fun fact 2" });

            found.ProductionCompanies.Should().HaveCount(2);
            found.ProductionCompanies.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
            found.ProductionCompanies.Select(e => e.Name).Should().BeEquivalentTo(new[] { "Test production company 1", "Test production company 2" });

            found.Writers.Should().HaveCount(2);
            found.Writers.Select(e => e.Key).All(key => key != default(Guid)).Should().BeTrue();
            found.Writers.Select(e => e.FullName).Should().BeEquivalentTo(new[] { "Test writer 1", "Test writer 2" });
        }
    }
}
