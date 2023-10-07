using Delp.Application.Abstractions;
using Delp.Application.DTOs.Authorization;
using Delp.Application.Repositories;
using Delp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Delp.Application.Implementations
{
    public class JwtFactory : IJwtFactory
    {
        private readonly IConfiguration _configurations;
        private readonly IUserRepository _userRepository;

        public JwtFactory(IConfiguration configurations, IUserRepository userManager)
        {
            _configurations = configurations;
            _userRepository = userManager;
        }

        public async Task<AuthResultDTO> GetAccessToken(User user, List<string> roles, bool staySignedIn = false)
        {
            var jwt = CreateToken(user, roles);
            var refreshToken = string.Empty;
            if (staySignedIn)
            {
                refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userRepository.UpdateAsync(user);
            }

            return new AuthResultDTO
            {
                RefreshToken = refreshToken,
                Token = jwt,
                Username = user.UserName
            };
        }
        public async Task<AuthResultDTO> RefreshToken(string oldToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(oldToken);
            var id = principal?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var user = await _userRepository.GetUserById(id);
            var roles = await _userRepository.GetUserRoles(user);

            if (user != null && user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.Now)
            {
            }
            var newToken = CreateToken(user, roles);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userRepository.UpdateAsync(user);
            return new AuthResultDTO
            {
                RefreshToken = newRefreshToken,
                Token = newToken,
                Username = user.UserName
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private string CreateToken(User user, List<string> roles)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configurations["JwtSettings:Key"]);
            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("userId",user.Id.ToString())
            };

            foreach (var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var lifeTime = TimeSpan.FromHours(int.Parse(_configurations["JwtSettings:LifeTime"].ToString()));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(lifeTime),
                Issuer = _configurations["JwtSettings:Issuer"],
                Audience = _configurations["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = handler.CreateToken(tokenDescriptor);
            var jwt = handler.WriteToken(token);
            return jwt;
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _configurations["JwtSettings:Issuer"],
                ValidAudience = _configurations["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurations["JwtSettings:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}