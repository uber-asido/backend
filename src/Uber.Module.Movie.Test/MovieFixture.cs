using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uber.Core.Test;
using Uber.Module.Movie.EFCore;
using Xunit;

namespace Uber.Module.Movie.Test
{
    public class MovieFixture : ServiceFixture
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=localhost;Port=5432;Database=uber_movie_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddMovie(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));
        }
    }

    [CollectionDefinition(Name)]
    public class MovieTestCollection : ICollectionFixture<MovieFixture>
    {
        public const string Name = "Movie test collection";
    }
}
