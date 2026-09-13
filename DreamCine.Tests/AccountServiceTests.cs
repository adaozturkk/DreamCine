using DreamCine.Application.DTOs.Account;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Services;
using DreamCine.Core.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DreamCine.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            var storeMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(
                storeMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
            );
            _tokenServiceMock = new Mock<ITokenService>();
            _emailServiceMock = new Mock<IEmailService>();
            _service = new AccountService(
                _userManagerMock.Object,
                _tokenServiceMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_Success_ReturnsSuccess()
        {
            _userManagerMock.Setup(um =>
                um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(um =>
                um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            var dto = new RegisterDto
            {
                Email = "test@gmail.com",
                Password = "testPassword123!",
                Username = "testuser"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("User created successfully and assigned to 'User' role.", result.Data);
        }

        [Fact]
        public async Task RegisterAsync_UserCreationFailed_ReturnsBadRequest()
        {
            var failedResult = IdentityResult.Failed(new IdentityError
            {
                Description = "Username is already taken."
            });

            _userManagerMock.Setup(um =>
                um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(failedResult);

            var dto = new RegisterDto
            {
                Email = "test@gmail.com",
                Password = "testPassword123!",
                Username = "testuser"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Username is already taken.", result.ErrorMessage);
        }

        [Fact]
        public async Task RegisterAsync_RoleAssignmentFailed_ReturnsInternalServerError()
        {
            _userManagerMock.Setup(um =>
                um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var failedRoleResult = IdentityResult.Failed(new IdentityError
            {
                Description = "Role assignment failed."
            });

            _userManagerMock.Setup(um =>
                um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
                .ReturnsAsync(failedRoleResult);

            var dto = new RegisterDto
            {
                Email = "test@gmail.com",
                Password = "testPassword123!",
                Username = "testuser"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Role assignment failed.", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokens()
        {
            var user = new AppUser
            {
                Email = "test@gmail.com",
                UserName = "testuser"
            };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.CheckPasswordAsync(user, "testPassword123!"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(um =>
                um.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _tokenServiceMock.Setup(ts =>
                ts.CreateToken(user))
                .ReturnsAsync("fake-access-token");

            _tokenServiceMock.Setup(ts =>
                ts.GenerateRefreshToken())
                .Returns("fake-refresh-token");

            var dto = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "testPassword123!"
            };

            var result = await _service.LoginAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("fake-access-token", result.Data!.AccessToken);
            Assert.Equal("fake-refresh-token", result.Data!.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsUnauthorized()
        {
            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync((AppUser?)null);

            var dto = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "testPassword123!"
            };

            var result = await _service.LoginAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid email or password.", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsUnauthorized()
        {
            var user = new AppUser
            {
                Email = "test@gmail.com",
                UserName = "testuser"
            };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.CheckPasswordAsync(user, "wrong-password"))
                .ReturnsAsync(false);

            var dto = new LoginDto
            {
                Email = "test@gmail.com",
                Password = "wrong-password"
            };

            var result = await _service.LoginAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid email or password.", result.ErrorMessage);
        }

        [Fact]
        public async Task RefreshTokenAsync_Success_ReturnsNewTokens()
        {
            var user = new AppUser
            {
                Email = "test@gmail.com",
                RefreshToken = "valid-token",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
            };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _tokenServiceMock.Setup(ts =>
                ts.CreateToken(user))
                .ReturnsAsync("fake-access-token");

            _tokenServiceMock.Setup(ts =>
                ts.GenerateRefreshToken())
                .Returns("fake-refresh-token");

            var dto = new TokenDto
            {
                Email = "test@gmail.com",
                RefreshToken = "valid-token"
            };

            var result = await _service.RefreshTokenAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("fake-access-token", result.Data!.AccessToken);
            Assert.Equal("fake-refresh-token", result.Data!.RefreshToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_InvalidOrExpiredToken_ReturnsBadRequest()
        {
            var user = new AppUser
            {
                Email = "test@gmail.com",
                RefreshToken = "valid-token",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(-1)
            };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            var dto = new TokenDto
            {
                Email = "test@gmail.com",
                RefreshToken = "valid-token"
            };

            var result = await _service.RefreshTokenAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid or expired refresh token. Please log in again.", result.ErrorMessage);
        }

        [Fact]
        public async Task AssignRoleAsync_UserNotFound_ReturnsNotFound()
        {
            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("notfound@gmail.com"))
                .ReturnsAsync((AppUser?)null);

            var dto = new AssignRoleDto
            {
                Email = "notfound@gmail.com",
                Role = DreamCine.Core.Enums.UserRoles.Admin
            };

            var result = await _service.AssignRoleAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Email not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task AssignRoleAsync_Success_ReturnsSuccess()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _userManagerMock.Setup(um =>
                um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(um =>
                um.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var dto = new AssignRoleDto
            {
                Email = "test@gmail.com",
                Role = DreamCine.Core.Enums.UserRoles.Admin
            };

            var result = await _service.AssignRoleAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Role assigned successfully.", result.Data);
        }

        [Fact]
        public async Task AssignRoleAsync_AddToRoleFailed_ReturnsBadRequest()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _userManagerMock.Setup(um =>
                um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            var failedResult = IdentityResult.Failed(new IdentityError
            {
                Description = "Role assignment failed."
            });

            _userManagerMock.Setup(um =>
                um.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(failedResult);

            var dto = new AssignRoleDto
            {
                Email = "test@gmail.com",
                Role = DreamCine.Core.Enums.UserRoles.Admin
            };

            var result = await _service.AssignRoleAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Role assignment failed.", result.ErrorMessage);
        }

        [Fact]
        public async Task ChangePasswordAsync_UserNotFound_ReturnsBadRequest()
        {
            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("notfound@gmail.com"))
                .ReturnsAsync((AppUser?)null);

            var dto = new ChangePasswordDto
            {
                CurrentPassword = "oldPassword123!",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ChangePasswordAsync(dto, "notfound@gmail.com");

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid request.", result.ErrorMessage);
        }

        [Fact]
        public async Task ChangePasswordAsync_Success_ReturnsSuccess()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.ChangePasswordAsync(user, "oldPassword123!", "newPassword123!"))
                .ReturnsAsync(IdentityResult.Success);

            var dto = new ChangePasswordDto
            {
                CurrentPassword = "oldPassword123!",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ChangePasswordAsync(dto, "test@gmail.com");

            Assert.True(result.IsSuccess);
            Assert.Equal("Password changed successfully.", result.Data);
        }

        [Fact]
        public async Task ChangePasswordAsync_Failed_ReturnsBadRequest()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            var failedResult = IdentityResult.Failed(new IdentityError
            {
                Description = "Incorrect password."
            });

            _userManagerMock.Setup(um =>
                um.ChangePasswordAsync(user, "wrongPassword123!", "newPassword123!"))
                .ReturnsAsync(failedResult);

            var dto = new ChangePasswordDto
            {
                CurrentPassword = "wrongPassword123!",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ChangePasswordAsync(dto, "test@gmail.com");

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Incorrect password.", result.ErrorMessage);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserNotFound_ReturnsSuccessWithoutSendingEmail()
        {
            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("notfound@gmail.com"))
                .ReturnsAsync((AppUser?)null);

            var dto = new ForgotPasswordDto
            {
                Email = "notfound@gmail.com"
            };

            var result = await _service.ForgotPasswordAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("If an account exists with this email, a password reset link has been sent.", result.Data);
            _emailServiceMock.Verify(e =>
                e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserExists_SendsEmailAndReturnsSuccess()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("reset-token-123");

            var dto = new ForgotPasswordDto
            {
                Email = "test@gmail.com"
            };

            var result = await _service.ForgotPasswordAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("If an account exists with this email, a password reset link has been sent.", result.Data);
            _emailServiceMock.Verify(e =>
                e.SendEmailAsync("test@gmail.com", "Password Reset Request", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound_ReturnsBadRequest()
        {
            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("notfound@gmail.com"))
                .ReturnsAsync((AppUser?)null);

            var dto = new ResetPasswordDto
            {
                Email = "notfound@gmail.com",
                Token = "some-token",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ResetPasswordAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid request.", result.ErrorMessage);
        }

        [Fact]
        public async Task ResetPasswordAsync_Success_ReturnsSuccess()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(um =>
                um.ResetPasswordAsync(user, "valid-token", "newPassword123!"))
                .ReturnsAsync(IdentityResult.Success);

            var dto = new ResetPasswordDto
            {
                Email = "test@gmail.com",
                Token = "valid-token",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ResetPasswordAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Password has been reset successfully.", result.Data);
        }

        [Fact]
        public async Task ResetPasswordAsync_InvalidToken_ReturnsBadRequest()
        {
            var user = new AppUser { Email = "test@gmail.com" };

            _userManagerMock.Setup(um =>
                um.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync(user);

            var failedResult = IdentityResult.Failed(new IdentityError
            {
                Description = "Invalid token."
            });

            _userManagerMock.Setup(um =>
                um.ResetPasswordAsync(user, "invalid-token", "newPassword123!"))
                .ReturnsAsync(failedResult);

            var dto = new ResetPasswordDto
            {
                Email = "test@gmail.com",
                Token = "invalid-token",
                NewPassword = "newPassword123!"
            };

            var result = await _service.ResetPasswordAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid token.", result.ErrorMessage);
        }
    }
}
