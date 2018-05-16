using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Core.Test;
using Uber.Core.Test.Mock;
using Uber.Module.File.EFCore;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;
using Xunit;

namespace Uber.Module.File.Test
{
    public class FileFixture : ServiceFixture, IDisposable
    {
        private IServiceScope scope;
        private BackgroundJobServer jobServer;

        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=localhost;Port=5432;Database=uber_file_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddFile(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));
            services.AddSingleton<IGeocodingService>(new GeocodingServiceMock());
            services.AddSingleton<IMovieService>(new MovieServiceMock());
            services.AddHangfireServer("Server=localhost;Port=5432;Database=uber_hangfire_test;User Id=hangfire;Password=x;");
        }

        protected override void Configure()
        {
            scope = RootServiceProvider.CreateScope();
            jobServer = new BackgroundJobServer(
                new BackgroundJobServerOptions(),
                scope.ServiceProvider.GetRequiredService<JobStorage>());
        }

        public void Dispose()
        {
            jobServer.SendStop();
            jobServer.Dispose();
            scope.Dispose();
        }
    }

    [CollectionDefinition(Name)]
    public class FileTestCollection : ICollectionFixture<FileFixture>
    {
        public const string Name = "File test collection";
    }
}
