using Microsoft.EntityFrameworkCore;
using System;
using Uber.Module.Movie.Search.Abstraction;
using Uber.Module.Movie.Search.Abstraction.Store;
using Uber.Module.Movie.Search.EFCore;
using Uber.Module.Movie.Search.EFCore.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MovieSearchBuilderExtension
    {
        public static IMovieSearchBuilder UseEFCoreStores(this IMovieSearchBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services
                .AddDbContext<DataContext>(optionsAction)
                .AddStore<ISearchItemStore, SearchItemStore>();

            return builder;
        }
    }
}
