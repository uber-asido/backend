using Microsoft.EntityFrameworkCore;
using System;
using Uber.Module.File.Abstraction;
using Uber.Module.File.Abstraction.Store;
using Uber.Module.File.EFCore;
using Uber.Module.File.EFCore.Setup;
using Uber.Module.File.EFCore.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileBuilderExtension
    {
        public static IFileBuilder UseEFCoreStores(this IFileBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services
                .AddInstallerStep<Migrate>()
                .AddDbContext<DataContext>(optionsAction)
                .AddDataStore<DataStore, DataContext>()
                .AddStore<IUploadHistoryStore, UploadHistoryStore>();

            return builder;
        }
    }
}
