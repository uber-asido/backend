using Microsoft.EntityFrameworkCore;
using Uber.Module.Movie.Search.Abstraction.Model;

namespace Uber.Module.Movie.Search.EFCore
{
    internal class DataContext : DbContext
    {
        public virtual DbSet<SearchItem> SearchItems { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SearchItem>(config =>
            {
                config.ToTable("search_item", "movie_search");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Text).HasColumnName("text");
                config.Property(e => e.Type).HasColumnName("type");
            });
        }
    }
}
