﻿using System;
using Uber.Module.Movie.Search.Abstraction;
using Uber.Module.Movie.Search.Abstraction.Manager;
using Uber.Module.Movie.Search.Manager;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class MovieSearchBuilder : IMovieSearchBuilder
    {
        public IServiceCollection Services { get; }

        public MovieSearchBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMovieSearch(this IServiceCollection services, Action<IMovieSearchBuilder> configureAction)
        {
            services
                .AddScoped<SearchManager>()
                .AddManager<ISearchManager, SearchManager>();

            var builder = new MovieSearchBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
