using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.File.Abstraction.Service;

namespace Uber.Module.File.Test
{
    public class FileTestBase : IDisposable
    {
        public readonly IFileService FileService;

        private readonly IServiceScope scope;

        public FileTestBase(FileFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            FileService = scope.ServiceProvider.GetRequiredService<IFileService>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
