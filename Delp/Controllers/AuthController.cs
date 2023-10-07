using Delp.Application.Abstractions;
using Delp.Application.DTOs.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            try
            {
                var result = await _authService.Login(loginDTO);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception)
            {
                //Log
                return StatusCode(500, "Internal Error while logging in");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {

            try
            {
                var result = await _authService.Register(dto);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Log
                return StatusCode(500, "Internal Error while register user");
            }
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> GenerateToken(string oldToken, string refreshToken)
        {
            try
            {
                var result = await _authService.RefreshToken(oldToken,refreshToken);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception)
            {
                //Log
                return StatusCode(500, "Internal Error while refresh token");
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var result = await _authService.ConfirmEmail(token, email);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Log
                return StatusCode(500, $"Internal Error while confirm email. ex : {ex.Message}");
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> GenerateToken(string email)
        {
            try
            {
                var result = await _authService.ResetPassword(email);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception)
            {
                //Log
                return StatusCode(500, "Internal Error while resetting password!");
            }
        }

        [HttpPost("confirm-password-reset")]
        public async Task<IActionResult> ConfirmEmail(ResetPasswordDTO dto)
        {
            try
            {
                var result = await _authService.ConfirmResetPassword(dto);

                if (result.IsSucceeded)
                {
                    return Ok(result.GetResponse());
                }

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Log
                return StatusCode(500, $"Internal Error while resetting password. ex : {ex.Message}");
            }
        }
    }
}