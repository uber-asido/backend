using Microsoft.EntityFrameworkCore;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.EFCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<SearchItem> SearchItems { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SearchItem>(config =>
            {
                config.ToTable("search_item");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Text).HasColumnName("text");
                config.Property(e => e.Type).HasColumnName("type");
                config.HasKey(e => e.Key);
            });
        }
    }
}
