using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uber.Core.Test;
using Uber.Module.File.EFCore;
using Xunit;

namespace Uber.Module.File.Test
{
    public class FileFixture : ServiceFixture
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=172.27.243.9;Port=5432;Database=uber_file_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddFile(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));
        }
    }

    [CollectionDefinition(Name)]
    public class FileTestCollection : ICollectionFixture<FileFixture>
    {
        public const string Name = "File test collection";
    }
}
