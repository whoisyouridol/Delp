﻿namespace Delp.Application.DTOs.Authorization;

public class RegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}