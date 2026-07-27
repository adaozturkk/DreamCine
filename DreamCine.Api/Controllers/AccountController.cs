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
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<TokenDto> _tokenValidator;
        private readonly IValidator<AssignRoleDto> _assignRoleValidator;
        private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService,  IValidator<RegisterDto> registerValidator,
             IValidator<LoginDto> loginValidator, IValidator<TokenDto> tokenValidator, IValidator<AssignRoleDto> assignRoleValidator, 
             IValidator<ForgotPasswordDto> forgotPasswordValidator, IValidator<ResetPasswordDto> resetPasswordValidator,
             IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _tokenValidator = tokenValidator;
            _assignRoleValidator = assignRoleValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _changePasswordValidator = changePasswordValidator;
            _accountService = accountService;
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

            var result = await _accountService.RegisterAsync(registerDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.LoginAsync(loginDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.RefreshTokenAsync(tokenDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.AssignRoleAsync(roleDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.ForgotPasswordAsync(passwordDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.ResetPasswordAsync(passwordDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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

            var result = await _accountService.ChangePasswordAsync(passwordDto, userEmail);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
