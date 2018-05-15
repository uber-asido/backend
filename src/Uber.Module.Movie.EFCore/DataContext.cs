using Microsoft.EntityFrameworkCore;

namespace Uber.Module.Movie.EFCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Entity.Movie> Movies { get; set; }

        public virtual DbSet<Entity.MovieActor> MovieActors { get; set; }
        public virtual DbSet<Entity.MovieDistributor> MovieDistributors { get; set; }
        public virtual DbSet<Entity.MovieProductionCompany> MovieProductionCompanies { get; set; }
        public virtual DbSet<Entity.MovieWriter> MovieWriters { get; set; }

        public virtual DbSet<Abstraction.Model.Actor> Actors { get; set; }
        public virtual DbSet<Abstraction.Model.Distributor> Distributors { get; set; }
        public virtual DbSet<Abstraction.Model.ProductionCompany> ProductionCompanies { get; set; }
        public virtual DbSet<Abstraction.Model.Writer> Writers { get; set; }

        public virtual DbSet<Entity.FilmingLocation> FilmingLocations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity.Movie>(config =>
            {
                config.ToTable("movie");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Title).HasColumnName("title");
                config.Property(e => e.ReleaseYear).HasColumnName("release_year");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Entity.MovieActor>(config =>
            {
                config.ToTable("movie_actor");
                config.Property(e => e.ActorKey).HasColumnName("actor_key");
                config.Property(e => e.MovieKey).HasColumnName("movie_key");
                config.HasKey(e => new { e.ActorKey, e.MovieKey });
            });

            modelBuilder.Entity<Entity.MovieDistributor>(config =>
            {
                config.ToTable("movie_distributor");
                config.Property(e => e.DistributorKey).HasColumnName("distributor_key");
                config.Property(e => e.MovieKey).HasColumnName("movie_key");
                config.HasKey(e => new { e.DistributorKey, e.MovieKey });
            });

            modelBuilder.Entity<Entity.FilmingLocation>(config =>
            {
                config.ToTable("filming_location");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.AddressKey).HasColumnName("address_key");
                config.Property(e => e.MovieKey).HasColumnName("movie_key");
                config.Property(e => e.FunFact).HasColumnName("fun_fact");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Entity.MovieProductionCompany>(config =>
            {
                config.ToTable("movie_production_company");
                config.Property(e => e.MovieKey).HasColumnName("movie_key");
                config.Property(e => e.ProductionCompanyKey).HasColumnName("production_company_key");
                config.HasKey(e => new { e.MovieKey, e.ProductionCompanyKey });
            });

            modelBuilder.Entity<Entity.MovieWriter>(config =>
            {
                config.ToTable("movie_writer");
                config.Property(e => e.MovieKey).HasColumnName("movie_key");
                config.Property(e => e.WriterKey).HasColumnName("writer_key");
                config.HasKey(e => new { e.MovieKey, e.WriterKey });
            });

            modelBuilder.Entity<Abstraction.Model.Actor>(config =>
            {
                config.ToTable("actor");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.FullName).HasColumnName("full_name");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Abstraction.Model.Distributor>(config =>
            {
                config.ToTable("distributor");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Name).HasColumnName("name");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Abstraction.Model.ProductionCompany>(config =>
            {
                config.ToTable("production_company");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Name).HasColumnName("name");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Abstraction.Model.Writer>(config =>
            {
                config.ToTable("writer");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.FullName).HasColumnName("full_name");
                config.HasKey(e => e.Key);
            });
        }
    }
}
