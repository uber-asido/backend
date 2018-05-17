using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Uber.Core.OData;
using Uber.Module.Movie.Abstraction.Model;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseMovieApi(this IApplicationBuilder app, ODataModelBuilder odataBuilder)
        {
            odataBuilder.AddNamespace("Movie", builder =>
            {
                var actor = builder.AddEntityType<Actor>();
                actor.HasKey(e => e.Key);

                var distributor = builder.AddEntityType<Distributor>();
                distributor.HasKey(e => e.Key);

                var productionCompany = builder.AddEntityType<ProductionCompany>();
                productionCompany.HasKey(e => e.Key);

                var writer = builder.AddEntityType<Writer>();
                writer.HasKey(e => e.Key);

                var filmingLocation = builder.AddEntitySet<FilmingLocation>();
                filmingLocation.EntityType.HasKey(e => e.Key);
                
                var movie = builder.AddEntitySet<Movie>();
                movie.EntityType.HasKey(e => e.Key);
                movie.EntityType.ContainsMany(e => e.Actors).AutoExpand = true;
                movie.EntityType.ContainsMany(e => e.Distributors).AutoExpand = true;
                movie.EntityType.ContainsMany(e => e.FilmingLocations).AutoExpand = true;
                movie.EntityType.ContainsMany(e => e.ProductionCompanies).AutoExpand = true;
                movie.EntityType.ContainsMany(e => e.Writers).AutoExpand = true;

                var filmingLocationSearchByFreeText = filmingLocation.EntityType.Collection.AddFunction("SearchByFreeText");
                filmingLocationSearchByFreeText.Parameter<string>("text");
                filmingLocationSearchByFreeText.ReturnsCollectionFromEntitySet<FilmingLocation>(nameof(FilmingLocation));
            });

            return app;
        }
    }
}
