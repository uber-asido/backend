using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Module.Movie.Test
{
    public class MovieTestBase : IDisposable
    {
        public readonly IMovieService MovieSerice;

        private readonly IServiceScope scope;

        public MovieTestBase(MovieFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            MovieSerice = scope.ServiceProvider.GetRequiredService<IMovieService>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
