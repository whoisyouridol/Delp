namespace Delp.Application.DTOs.Authorization
{
    public class AuthResultDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}