using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Core.Test;
using Uber.Module.File.EFCore;
using Xunit;

namespace Uber.Module.File.Test
{
    public class FileFixture : ServiceFixture, IDisposable
    {
        private BackgroundJobServer jobServer;
        
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=172.27.243.9;Port=5432;Database=uber_file_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddFile(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));

            services.AddHangfireActivators();
            JobStorage.Current = new PostgreSqlStorage("Server=172.27.243.9;Port=5432;Database=uber_hangfire_test;User Id=hangfire;Password=x;");
            jobServer = new BackgroundJobServer();
        }

        public void Dispose()
        {
            jobServer.Dispose();
        }
    }

    [CollectionDefinition(Name)]
    public class FileTestCollection : ICollectionFixture<FileFixture>
    {
        public const string Name = "File test collection";
    }
}
