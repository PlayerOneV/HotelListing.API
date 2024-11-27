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
        
        // api/Authentication/register
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto) {
            var loginResult = await _authManager.Login(loginDto);
            return loginResult switch
            {
                UserLoginResult.Success => Ok(),
                UserLoginResult.UserNotFound => NotFound(new { message = "User not found" }),
                UserLoginResult.InvalidPassword => Unauthorized(new { message = "Invalid password" }),
                _ => StatusCode(500, new { message = "Unexpected error occurred" })
            };
        }
    }
}
