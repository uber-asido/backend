using System;
using Uber.Module.File.Abstraction;
using Uber.Module.File.Abstraction.Service;
using Uber.Module.File.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class FileBuilder : IFileBuilder
    {
        public IServiceCollection Services { get; }

        public FileBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFile(this IServiceCollection services, Action<IFileBuilder> configureAction)
        {
            services
                .AddService<IFileService, FileService>()
                .AddService<FileService, FileService>(); // Concrete implementation for background jobs.

            var builder = new FileBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
