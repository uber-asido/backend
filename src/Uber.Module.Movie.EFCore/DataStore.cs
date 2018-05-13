using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;

namespace Uber.Module.Movie.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<Entity.Movie> Movies => DataContext.Movies;

        public DbSet<Entity.MovieActor> MovieActors => DataContext.MovieActors;
        public DbSet<Entity.MovieDistributor> MovieDistributors => DataContext.MovieDistributors;
        public DbSet<Entity.MovieProductionCompany> MovieProductionCompanies => DataContext.MovieProductionCompanies;
        public DbSet<Entity.MovieWriter> MovieWriters => DataContext.MovieWriters;

        public DbSet<Abstraction.Model.Actor> Actors => DataContext.Actors;
        public DbSet<Abstraction.Model.Distributor> Distributors => DataContext.Distributors;
        public DbSet<Abstraction.Model.ProductionCompany> ProductionCompanies => DataContext.ProductionCompanies;
        public DbSet<Abstraction.Model.Writer> Writers => DataContext.Writers;

        public DbSet<Entity.FilmingLocation> FilmingLocations => DataContext.FilmingLocations;

        public DataStore(DataContext context) : base(context) { }
    }
}
