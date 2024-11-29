using HotelListing.API.Contracts;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthenticationController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        // api/Authentication/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] UserDto userDto) {
            var errors = await _authManager.Register(userDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }
        
        // api/Authentication/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessfullAuthenticationDto>> Login([FromBody] LoginDto loginDto) {
            
            var loginResult = await _authManager.Login(loginDto);
            
            switch (loginResult.LoginResponse)
            {
                case UserLoginResponse.Success:
                    var token = await _authManager.GenerateJwtToken(loginResult.LoggedUser);
                    var refreshToken = await _authManager.CreateRefreshToken(loginResult.LoggedUser);
                    return Ok(new SuccessfullAuthenticationDto
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                        UserId = loginResult.LoggedUser.Id
                    });
                case UserLoginResponse.UserNotFound:
                    return NotFound();
                case UserLoginResponse.InvalidPassword:
                    return Unauthorized();
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        } 
        
        // api/Authentication/refresh-token
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessfullAuthenticationDto>> RefreshToken([FromBody] SuccessfullAuthenticationDto successfullAuthenticationDto) {
            
            var refreshTokenResult = await _authManager.VerifyRefreshToken(successfullAuthenticationDto);
            
            if(refreshTokenResult is null) return Unauthorized();

            return Ok(refreshTokenResult);
        }
    }
}
