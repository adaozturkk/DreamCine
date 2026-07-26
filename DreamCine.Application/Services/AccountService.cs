using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Account;
using DreamCine.Application.Interfaces;
using DreamCine.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Channels;

namespace DreamCine.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<ServiceResult<string>> AssignRoleAsync(AssignRoleDto roleDto)
        {
            var user = await _userManager.FindByEmailAsync(roleDto.Email);
            if (user == null)
            {
                return ServiceResult<string>.Failure("Email not found.", 404);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var assignedRole = await _userManager.AddToRoleAsync(user, roleDto.Role.ToString());
            if (!assignedRole.Succeeded)
            {
                return ServiceResult<string>.Failure(string.Join(", ", assignedRole.Errors.Select(e => e.Description)), 400);
            }

            return ServiceResult<string>.Success("Role assigned successfully.", 200);
        }

        public async Task<ServiceResult<string>> ChangePasswordAsync(ChangePasswordDto passwordDto, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return ServiceResult<string>.Failure("Invalid request.", 400);
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return ServiceResult<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)), 400);
            }

            return ServiceResult<string>.Success("Password changed successfully.", 200);
        }

        public async Task<ServiceResult<string>> ForgotPasswordAsync(ForgotPasswordDto passwordDto)
        {
            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null)
            {
                return ServiceResult<string>.Success("If an account exists with this email, a password reset link has been sent.", 200);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:3000/reset-password?email={user.Email}&token={token}";
            string body = $"<h2>Password Reset Request</h2><p>To reset your password, please click the link below:</p><a href='{resetLink}'>Click here to reset your password</a>";

            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", body);

            return ServiceResult<string>.Success("If an account exists with this email, a password reset link has been sent.", 200);
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return ServiceResult<AuthResponseDto>.Failure("Invalid email or password.", 401);
            }

            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return ServiceResult<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }, 200);
        }

        public async Task<ServiceResult<AuthResponseDto>> RefreshTokenAsync(TokenDto tokenDto)
        {
            var user = await _userManager.FindByEmailAsync(tokenDto.Email);
            if (user == null)
            {
                return ServiceResult<AuthResponseDto>.Failure("User not found.", 400);
            }

            if (user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return ServiceResult<AuthResponseDto>.Failure("Invalid or expired refresh token. Please log in again.", 400);
            }

            var newAccessToken = await _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return ServiceResult<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }, 200);
        }

        public async Task<ServiceResult<string>> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                {
                    return ServiceResult<string>.Success("User created successfully and assigned to 'User' role.", 200);
                }
                else
                {
                    return ServiceResult<string>.Failure(string.Join(", ", roleResult.Errors.Select(e => e.Description)), 500);
                }
            }

            return ServiceResult<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)), 400);
        }

        public async Task<ServiceResult<string>> ResetPasswordAsync(ResetPasswordDto passwordDto)
        {
            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null)
            {
                return ServiceResult<string>.Failure("Invalid request.", 400);
            }

            var result = await _userManager.ResetPasswordAsync(user, passwordDto.Token, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return ServiceResult<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)), 400);
            }

            return ServiceResult<string>.Success("Password has been reset successfully.", 200);
        }
    }
}
