namespace Delp.Application.DTOs.Authorization
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}