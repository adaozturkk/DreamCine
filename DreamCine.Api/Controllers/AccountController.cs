using DreamCine.Api.DTOs.Account;
using DreamCine.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AccountController(UserManager<IdentityUser> userManager, ITokenService tokenService,
            IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = new IdentityUser
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
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordCheck)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = await _tokenService.CreateToken(user);

            return Ok(new
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            });
        }
    }
}
