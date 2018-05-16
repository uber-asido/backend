using Microsoft.EntityFrameworkCore;
using System;
using Uber.Module.Geocoding.Abstraction;
using Uber.Module.Geocoding.Abstraction.Store;
using Uber.Module.Geocoding.EFCore;
using Uber.Module.Geocoding.EFCore.Setup;
using Uber.Module.Geocoding.EFCore.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GeocodingBuilderExtension
    {
        public static IGeocodingBuilder UseEFCoreStores(this IGeocodingBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services
                .AddInstallerStep<Migrate>()
                .AddDbContext<DataContext>(optionsAction)
                .AddDataStore<DataStore, DataContext>()
                .AddStore<IGeocodingStore, GeocodingStore>()
                .AddStore<INotFoundStore, NotFoundStore>();

            return builder;
        }
    }
}
