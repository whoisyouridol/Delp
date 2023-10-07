using Delp.Domain.Entities;
namespace Delp.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> UserExistsAsync(string email);
    Task<bool> AddAsync(User user, string password);
    Task<bool> VerifyUser(string email, string password);
    Task UpdateAsync(User user);
    Task<List<string>> GetUserRoles(User user);
    Task<User> GetUserById(string id);
    Task<User> GetUserByEmail(string email);
    Task<string> CreateEmailVerificationToken(User user);
    Task<bool> ConfirmUser(User user, string token);
    Task<string> CreateResetPasswordToken(User user);
    Task<bool> ConfirmPasswordReset(User user,string newPassword, string token);
}