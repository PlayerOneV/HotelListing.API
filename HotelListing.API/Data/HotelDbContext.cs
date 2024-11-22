using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Brazil",
                    ShortName = "Bz"
                },new Country
                {
                    Id = 2,
                    Name = "Mexico",
                    ShortName = "Mx"
                },new Country
                {
                    Id = 3,
                    Name = "Japan",
                    ShortName = "Jp"
                }
             );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "The Rolnaldhino",
                    Address = "Sao Paolo",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Hinata",
                    Address = "Kyoto",
                    Rating = 4.3,
                    CountryId = 3
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Xcaret",
                    Address = "Riviera Maya",
                    Rating = 4.7,
                    CountryId = 2
                }
                );
        }
    }
}
