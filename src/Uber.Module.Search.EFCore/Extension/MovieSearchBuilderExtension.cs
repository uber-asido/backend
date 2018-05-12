using Microsoft.EntityFrameworkCore;
using System;
using Uber.Module.Search.Abstraction;
using Uber.Module.Search.Abstraction.Store;
using Uber.Module.Search.EFCore;
using Uber.Module.Search.EFCore.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MovieSearchBuilderExtension
    {
        public static IMovieSearchBuilder UseEFCoreStores(this IMovieSearchBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services
                .AddInstallerStep<Migrate>()
                .AddDbContext<DataContext>(optionsAction)
                .AddDataStore<DataStore, DataContext>()
                .AddStore<ISearchItemStore, SearchItemStore>();

            return builder;
        }
    }
}
