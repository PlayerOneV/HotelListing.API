using HotelListing.API.Data;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserDto userDto);
        Task<LoginResultDto> Login(LoginDto loginDto);
        Task<String> GenerateJwtToken(User user);
    }
}
