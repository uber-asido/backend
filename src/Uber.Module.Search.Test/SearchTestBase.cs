using Microsoft.Extensions.DependencyInjection;
using System;
using Uber.Module.Search.Abstraction.Manager;

namespace Uber.Module.Search.Test
{
    public class SearchTestBase : IDisposable
    {
        public readonly ISearchManager SearchManager;

        private readonly IServiceScope scope;

        public SearchTestBase(SearchFixture fixture)
        {
            scope = fixture.RootServiceProvider.CreateScope();
            SearchManager = scope.ServiceProvider.GetRequiredService<ISearchManager>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
