namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 
        }

        public ArtilleryContext(DbContextOptions options)
            : base(options) 
        { 
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;

        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        public virtual DbSet<Gun> Guns { get; set; } = null!;

        public virtual DbSet<CountryGun> CountriesGuns { get; set; } = null!;

        public virtual DbSet<Shell> Shells { get; set; } = null!;

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasIndex(p => p.ManufacturerName).IsUnique();
            });

            modelBuilder.Entity<CountryGun>(entity =>
            {
                entity.HasKey(k => new { k.CountryId, k.GunId });
            });
        }
    }
}
