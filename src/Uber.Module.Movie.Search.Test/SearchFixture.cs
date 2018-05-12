﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uber.Core.Test;
using Uber.Module.Movie.Search.EFCore;
using Xunit;

namespace Uber.Module.Movie.Search.Test
{
    public class SearchFixture : ServiceFixture
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=localhost;Port=5432;Database=uber_movie_search_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddMovieSearch(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));
        }
    }

    [CollectionDefinition(Name)]
    public class SearchTestCollection : ICollectionFixture<SearchFixture>
    {
        public const string Name = "Search test collection";
    }
}