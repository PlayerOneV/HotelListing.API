using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
    public class AuthManagerRepository : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private const string _loginProvider = "HotelListingAPI";
        private const string _refreshToken = "RefreshToken";

        public AuthManagerRepository(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles =  await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoggingAttentResultDto> Login(LoginDto loginDto)
        {
          
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            
            return await ValidateLoginResponse(user, loginDto.Password);
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



        private async Task<LoggingAttentResultDto> ValidateLoginResponse(User user, string password)
        {
            if (user is null)
            {
                return new LoggingAttentResultDto
                {
                    LoginResponse = UserLoginResponse.UserNotFound,
                    LoggedUser = null
                };
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return new LoggingAttentResultDto
                {
                    LoginResponse = UserLoginResponse.InvalidPassword,
                    LoggedUser = null
                };
            }

            return new LoggingAttentResultDto { LoginResponse = UserLoginResponse.Success, LoggedUser = user };
        }

        public async Task<string> CreateRefreshToken(User user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _loginProvider, _refreshToken);
            await _userManager.SetAuthenticationTokenAsync(user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<SuccessfullAuthenticationDto?> VerifyRefreshToken(SuccessfullAuthenticationDto refreshRequest)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(refreshRequest.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;

            var user = await _userManager.FindByNameAsync(username);

            if (user is null) return null;

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user, _loginProvider, _refreshToken, refreshRequest.RefreshToken);

            if (isValidRefreshToken)
            {
                return new SuccessfullAuthenticationDto
                {
                    Token = await GenerateJwtToken(user),
                    RefreshToken = await CreateRefreshToken(user),
                    UserId = user.Id,
                };
                
            }
            // invalid refresh token -> unauthorized
            return null;
        }
    }
}
