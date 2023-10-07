namespace Delp.Application.DTOs.Authorization;

public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool StaySignedIn { get; set; }
}