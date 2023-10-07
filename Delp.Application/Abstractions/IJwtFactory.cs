using Delp.Application.DTOs.Authorization;
using Delp.Domain.Entities;

namespace Delp.Application.Abstractions
{
    public interface IJwtFactory
    {
        Task<AuthResultDTO> GetAccessToken(User user, List<string> roles,bool staySignedIn = false);
        Task<AuthResultDTO> RefreshToken(string oldToken, string refreshToken);
    }
}