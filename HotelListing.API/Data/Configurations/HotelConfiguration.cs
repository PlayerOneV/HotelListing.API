using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {

            builder.HasData(
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
