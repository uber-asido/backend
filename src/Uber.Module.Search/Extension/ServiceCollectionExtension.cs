using System;
using Uber.Module.Search.Abstraction;
using Uber.Module.Search.Abstraction.Manager;
using Uber.Module.Search.Manager;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class SearchBuilder : ISearchBuilder
    {
        public IServiceCollection Services { get; }

        public SearchBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSearch(this IServiceCollection services, Action<ISearchBuilder> configureAction)
        {
            services
                .AddScoped<SearchManager>()
                .AddManager<ISearchManager, SearchManager>();

            var builder = new SearchBuilder(services);
            configureAction(builder);

            return services;
        }
    }
}
