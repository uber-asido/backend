using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Movie.Location.Abstraction;
using Uber.Module.Movie.Location.Abstraction.Store;
using Uber.Module.Movie.Location.EFCore.Setup;
using Uber.Module.Movie.Location.EFCore.Store;

namespace Uber.Module.Movie.Location.EFCore.Extension
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
