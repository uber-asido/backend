using Microsoft.Extensions.DependencyInjection;
using Uber.Module.Movie.Search.Manager;
using Xunit;

namespace Uber.Module.Movie.Search.Test
{
    public class SearchTestBase
    {
        public readonly SearchManager SearchManager;

        public SearchTestBase(SearchFixture fixture)
        {
            using (var scope = fixture.RootServiceProvider.CreateScope())
            {
                SearchManager = scope.ServiceProvider.GetRequiredService<SearchManager>();
            }
        }
    }
}
