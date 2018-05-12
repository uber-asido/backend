using System;
using Uber.Module.Movie.Abstraction;
using Uber.Module.Movie.Abstraction.Manager;
using Uber.Module.Movie.Manager;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class MovieBuilder : IMovieBuilder
    {
        public IServiceCollection Services { get; }

        public MovieBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMovie(this IServiceCollection services, Action<IMovieBuilder> configureAction)
        {
            services
                .AddScoped<MovieManager>()
                .AddManager<IMovieManager, MovieManager>();

            var builder = new MovieBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
