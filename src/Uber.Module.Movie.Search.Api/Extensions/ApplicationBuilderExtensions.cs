using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Uber.Core.OData;
using Uber.Module.Movie.Search.Abstraction.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMovieSearchApi(this IApplicationBuilder app, ODataModelBuilder odataBuilder)
        {
            odataBuilder.AddNamespace("MovieSearch", builder =>
            {
                builder.AddEnumType<SearchItemType>();

                var searchItem = builder.AddEntitySet<SearchItem>();
                searchItem.EntityType.HasKey(e => e.Key);
            });

            return app;
        }
    }
}
