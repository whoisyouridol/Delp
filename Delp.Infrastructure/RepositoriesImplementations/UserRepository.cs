using Delp.Application.Repositories;
using Delp.Domain.Entities;
using Delp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Delp.Infrastructure.RepositoriesImplementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

        }

        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return user is not null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return true;

            return false;
        }
        public async Task<List<string>> GetUserRoles(User user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }
        public async Task UpdateAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> VerifyUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<string> CreateEmailVerificationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<bool> ConfirmUser(User user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        public async Task<string> CreateResetPasswordToken(User user)
        {
           return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> ConfirmPasswordReset(User user, string newPassword, string token)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}