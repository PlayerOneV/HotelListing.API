﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "Brazil",
                    ShortName = "Bz"
                }, 
                new Country
                {
                    Id = 2,
                    Name = "Mexico",
                    ShortName = "Mx"
                }, 
                new Country
                {
                    Id = 3,
                    Name = "Japan",
                    ShortName = "Jp"
                }
             );
        }
    }
}
