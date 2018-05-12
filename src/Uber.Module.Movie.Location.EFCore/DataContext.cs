using Microsoft.EntityFrameworkCore;

namespace Uber.Module.Movie.Location.EFCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Entity.Movie> Movies { get; set; }
        public virtual DbSet<Abstraction.Model.Actor> Actors { get; set; }
        public virtual DbSet<Abstraction.Model.Distributor> Distributors { get; set; }
        public virtual DbSet<Abstraction.Model.ProductionCompany> ProductionCompanies { get; set; }
        public virtual DbSet<Abstraction.Model.Writer> Writers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity.Movie>(config =>
            {
                config.ToTable("movie");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Title).HasColumnName("title");
                config.Property(e => e.ReleaseYear).HasColumnName("release_year");
                config.Property(e => e.FunFacts).HasColumnName("fun_facts");
                config.HasKey(e => e.Key);
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
