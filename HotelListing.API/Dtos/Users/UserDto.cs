using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Dtos.Users
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        [StringLength(15, ErrorMessage ="Your password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
