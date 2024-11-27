using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Repository
{
    public class AuthManagerRepository : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AuthManagerRepository(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserLoginResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return UserLoginResult.UserNotFound;

            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!isValidPassword) return UserLoginResult.InvalidPassword;

            return UserLoginResult.Success;
        }

        public async Task<IEnumerable<IdentityError>> Register(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }
    }
}
