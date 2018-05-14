using Microsoft.EntityFrameworkCore;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.EFCore.Entity;

namespace Uber.Module.File.EFCore
{
    public class DataContext : DbContext
    {
        public virtual DbSet<UploadHistory> UploadHistories { get; set; }
        public virtual DbSet<FileData> FileData { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UploadHistory>(config =>
            {
                config.ToTable("upload_history");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Filename).HasColumnName("filename");
                config.Property(e => e.Status).HasColumnName("status");
                config.Property(e => e.Errors).HasColumnName("errors");
                config.Property(e => e.Timestamp).HasColumnName("timestamp");
                config.HasKey(e => e.Key);
            });

            modelBuilder.Entity<FileData>(config =>
            {
                config.ToTable("file_data");
                config.Property(e => e.Key).HasColumnName("key");
                config.Property(e => e.Data).HasColumnName("data");
                config.HasKey(e => e.Key);
            });
        }
    }
}
