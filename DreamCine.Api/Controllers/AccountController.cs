using Azure.Core;
using DreamCine.Application.DTOs.Account;
using DreamCine.Application.Interfaces;
using DreamCine.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<TokenDto> _tokenValidator;
        private readonly IValidator<AssignRoleDto> _assignRoleValidator;
        private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,  IValidator<RegisterDto> registerValidator,
             IValidator<LoginDto> loginValidator, IValidator<TokenDto> tokenValidator, IValidator<AssignRoleDto> assignRoleValidator, 
             IEmailService emailService, IValidator<ForgotPasswordDto> forgotPasswordValidator, IValidator<ResetPasswordDto> resetPasswordValidator,
             IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _tokenValidator = tokenValidator;
            _assignRoleValidator = assignRoleValidator;
            _emailService = emailService;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _changePasswordValidator = changePasswordValidator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
                    return Ok("User created successfully and assigned to 'User' role.");
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            var validationResult = await _tokenValidator.ValidateAsync(tokenDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(tokenDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid or expired refresh token. Please log in again.");
            }

            var newAccessToken = await _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto roleDto)
        {
            var validationResult = await _assignRoleValidator.ValidateAsync(roleDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(roleDto.Email);
            if (user == null)
            {
                return NotFound("Email not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var assignedRole = await _userManager.AddToRoleAsync(user, roleDto.Role.ToString());
            if (!assignedRole.Succeeded)
            {
                return BadRequest(assignedRole.Errors);
            }

            return Ok("Role assigned successfully.");
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto passwordDto)
        {
            var validationResult = await _forgotPasswordValidator.ValidateAsync(passwordDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null)
            {
                return Ok("If an account exists with this email, a password reset link has been sent.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:3000/reset-password?email={user.Email}&token={token}";
            string body = $"<h2>Password Reset Request</h2><p>To reset your password, please click the link below:</p><a href='{resetLink}'>Click here to reset your password</a>";

            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", body);

            return Ok("If an account exists with this email, a password reset link has been sent.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto passwordDto)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(passwordDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }

            var result = await _userManager.ResetPasswordAsync(user, passwordDto.Token, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password has been reset successfully.");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto passwordDto)
        {
            var validationResult = await _changePasswordValidator.ValidateAsync(passwordDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }
    }
}
