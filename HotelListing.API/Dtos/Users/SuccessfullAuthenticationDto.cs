namespace HotelListing.API.Dtos.Users
{
    public class SuccessfullAuthenticationDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
    }
}
