using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uber.Core.Test;
using Uber.Module.Search.EFCore;
using Xunit;

namespace Uber.Module.Search.Test
{
    public class SearchFixture : ServiceFixture
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new ConnectionString("Server=172.27.243.9;Port=5432;Database=uber_search_test;User Id=uber;Password=x;");
            services.AddSingleton(connectionString);
            services.AddSearch(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionString.Value)));
        }

        protected override void Configure() { }
    }

    [CollectionDefinition(Name)]
    public class SearchTestCollection : ICollectionFixture<SearchFixture>
    {
        public const string Name = "Search test collection";
    }
}
