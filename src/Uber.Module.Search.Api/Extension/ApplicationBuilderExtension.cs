using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Uber.Core.OData;
using Uber.Module.Search.Abstraction.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtension
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
