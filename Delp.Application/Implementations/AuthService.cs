using Delp.Application.Abstractions;
using Delp.Application.Commons;
using Delp.Application.DTOs.Authorization;
using Delp.Application.InfrastructureAbstractions;
using Delp.Application.Repositories;
using Delp.Domain.Entities;
using System.Web;

namespace Delp.Application.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwt;
        private readonly IMailService _mailService;
        public AuthService(IUserRepository userRepository, IJwtFactory jwt, IMailService mailService)
        {
            _userRepository = userRepository;
            _jwt = jwt;
            _mailService = mailService;
        }
        public async Task<ExecutionResult> Register(RegisterDTO dto)
        {
            if (!(dto.Password == dto.ConfirmPassword))
                return ExecutionResult<AuthResultDTO>.Fail("Inputed passwords are not same");

            var userExists = await _userRepository.UserExistsAsync(dto.Email);

            if (userExists)
                return ExecutionResult<AuthResultDTO>.Fail("User already exists");

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email.Split('@')[0],

            };

            var userCreated = await _userRepository.AddAsync(user, dto.Password);

            if (!userCreated)
                return ExecutionResult<AuthResultDTO>.Fail("Error while creating user");

            var confirmationToken = await _userRepository.CreateEmailVerificationToken(user);
            var email = new
            {
                Recipients = new[] {user.Email },
                Subject = "Verify your email in Delp",
                Content = $"Hello from Delp app! To use our system, please, follow this link to verify your account https://localhost:7205/api/Auth/confirm-email?token={HttpUtility.UrlEncode(confirmationToken)}&email={user.Email}"
            };
            await _mailService.SendMail(email.Recipients,email.Subject, email.Content);

            return ExecutionResult.Success();
        }
        public async Task<ExecutionResult<AuthResultDTO>> Login(LoginDTO dto)
        {
            var hasValidCredentials = await _userRepository.VerifyUser(dto.Email, dto.Password);
            if (!hasValidCredentials)
                return ExecutionResult<AuthResultDTO>.Fail("Email or password were incorrect!");

            var user = await _userRepository.GetUserByEmail(dto.Email);
            var roles = await _userRepository.GetUserRoles(user);
            var authResult = await _jwt.GetAccessToken(user, roles, dto.StaySignedIn);

            return authResult;
        }
        public async Task<ExecutionResult<AuthResultDTO>> RefreshToken(string oldToken, string refreshToken)
        {
            return await _jwt.RefreshToken(oldToken, refreshToken);
        }

        public async Task<ExecutionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return ExecutionResult.Fail("User Not Found");

            var confirmed = await _userRepository.ConfirmUser(user, token);
            if (confirmed)
                return ExecutionResult.Success();
            else
                return ExecutionResult.Fail("Error occured while confirmating user");
        }

        public async Task<ExecutionResult> ResetPassword(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return ExecutionResult.Fail("User Not Found");

            var resetPasswordToken = await _userRepository.CreateResetPasswordToken(user);
            var mail = new
            {
                Recipients = new[] { user.Email },
                Subject = "Reset password in Delp",
                Content = $"Hello from Delp app! To reset your password, please, follow this link to verify your account (this is test and here will be just token : {resetPasswordToken}"
            };
            await _mailService.SendMail(mail.Recipients, mail.Subject, mail.Content);

            return ExecutionResult.Success();
        }

        public async Task<ExecutionResult> ConfirmResetPassword(ResetPasswordDTO dto)
        {
            if (!(dto.Password == dto.ConfirmPassword))
                return ExecutionResult<AuthResultDTO>.Fail("Inputed passwords are not same");

            var user = await _userRepository.GetUserByEmail(dto.Email);

            if (user is null)
                return ExecutionResult<AuthResultDTO>.Fail("User Not Found");

            var passwordReseted = await _userRepository.ConfirmPasswordReset(user, dto.Password, dto.ResetToken);

            return passwordReseted ? ExecutionResult.Success() : ExecutionResult.Fail("Error while reseting password");
        }
    }
}