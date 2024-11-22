using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
