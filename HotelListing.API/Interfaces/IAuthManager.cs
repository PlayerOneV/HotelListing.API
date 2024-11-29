using HotelListing.API.Data;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserDto userDto);
        Task<LoggingAttentResultDto> Login(LoginDto loginDto);
        Task<String> GenerateJwtToken(User user);
        Task<String> CreateRefreshToken(User user);
        Task<SuccessfullAuthenticationDto?> VerifyRefreshToken(SuccessfullAuthenticationDto loginResultDto);
    }
}
