using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Movie.Manager;

namespace Uber.Module.Movie.Test
{
    public class MovieTestBase : IDisposable
    {
        public readonly MovieManager MovieManager;

        private readonly IServiceScope scope;

        public MovieTestBase(MovieFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            MovieManager = scope.ServiceProvider.GetRequiredService<MovieManager>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
