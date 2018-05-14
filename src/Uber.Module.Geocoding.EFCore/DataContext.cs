using Microsoft.EntityFrameworkCore;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.EFCore.Entity;

namespace Uber.Module.Geocoding.EFCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(config =>
            {
                config.ToTable("address");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.FormattedAddress).HasColumnName("formatted_address");
                config.Property(e => e.Latitude).HasColumnName("latitude");
                config.Property(e => e.Longitude).HasColumnName("longitude");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<Location>(config =>
            {
                config.ToTable("location");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.AddressKey).HasColumnName("address_key");
                config.Property(e => e.UnformattedAddress).HasColumnName("unformatted_address");
                config.HasKey(e => e.Key);
            });
        }
    }
}
