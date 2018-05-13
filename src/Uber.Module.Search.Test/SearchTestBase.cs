using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Search.Abstraction.Service;

namespace Uber.Module.Search.Test
{
    public class SearchTestBase : IDisposable
    {
        public readonly ISearchService SearchService;

        private readonly IServiceScope scope;

        public SearchTestBase(SearchFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            SearchService = scope.ServiceProvider.GetRequiredService<ISearchService>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
