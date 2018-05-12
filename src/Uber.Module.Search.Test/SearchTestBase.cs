using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Search.Manager;

namespace Uber.Module.Search.Test
{
    public class SearchTestBase : IDisposable
    {
        public readonly SearchManager SearchManager;

        private readonly IServiceScope scope;

        public SearchTestBase(SearchFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            SearchManager = scope.ServiceProvider.GetRequiredService<SearchManager>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
