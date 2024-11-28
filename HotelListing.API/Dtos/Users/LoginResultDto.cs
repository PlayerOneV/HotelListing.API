using HotelListing.API.Data;
using HotelListing.API.Enums;

namespace HotelListing.API.Dtos.Users
{
    public class LoginResultDto
    {
        public User LoggedUser { get; set; }
        public UserLoginResponse LoginResponse { get; set; }
    }
}
