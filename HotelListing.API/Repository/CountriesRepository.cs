using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelDbContext _context;
        public CountriesRepository(HotelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country?> GetDetails(int id)
        {
            return await _context.Countries.Include(country => country.Hotels)
                .FirstOrDefaultAsync(country => country.Id == id);
        }
    }
}
