using Delp.Application.Commons;
using Delp.Application.DTOs.Authorization;

namespace Delp.Application.Abstractions;

public interface IAuthService
{
    Task<ExecutionResult> Register(RegisterDTO dto);
    Task<ExecutionResult<AuthResultDTO>> Login(LoginDTO dto);
    Task<ExecutionResult<AuthResultDTO>> RefreshToken(string oldToken, string refreshToken);
    Task<ExecutionResult> ConfirmEmail(string token, string email);
    Task<ExecutionResult> ResetPassword(string email);
    Task<ExecutionResult> ConfirmResetPassword(ResetPasswordDTO dto);
}