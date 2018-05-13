using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Movie.Abstraction.Manager;

namespace Uber.Module.Movie.Test
{
    public class MovieTestBase : IDisposable
    {
        public readonly IMovieManager MovieManager;

        private readonly IServiceScope scope;

        public MovieTestBase(MovieFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            MovieManager = scope.ServiceProvider.GetRequiredService<IMovieManager>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
