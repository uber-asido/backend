using Microsoft.EntityFrameworkCore;
using System;
using Uber.Module.Movie.Abstraction;
using Uber.Module.Movie.Abstraction.Store;
using Uber.Module.Movie.EFCore;
using Uber.Module.Movie.EFCore.Setup;
using Uber.Module.Movie.EFCore.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FilmMovieBuilderExtension
    {
        public static IFilmMovieBuilder UseEFCoreStores(this IFilmMovieBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services
                .AddInstallerStep<Migrate>()
                .AddDbContext<DataContext>(optionsAction)
                .AddDataStore<DataStore, DataContext>()
                .AddStore<IMovieStore, MovieStore>();

            return builder;
        }
    }
}
