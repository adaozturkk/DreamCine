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
    }
}
