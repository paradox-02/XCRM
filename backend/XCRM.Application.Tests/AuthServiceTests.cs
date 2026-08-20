using Moq;
using XCRM.Application.Authentication;
using XCRM.Application.Authentication.DTOs;
using XCRM.Application.Common.Security;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Tests
{
    public sealed class AuthServiceTests
    {
        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ReturnNull()
        {
            var user = new Mock<ISysUserRepository>();
            var password = new Mock<IPasswordHasher>();
            var access = new Mock<IAccessTokenGenerator>();

            user.Setup(n => n.GetByUsernameAsync(
                "missing",
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((SysUser?)null);

            var service = new AuthService(
                user.Object,
                password.Object,
                access.Object);

            var request = new LoginRequest
            {
                Username = "missing",
                Password = "password123"
            };

            var result = await service.LoginAsync(request);

            Assert.Null(result);

            password.Verify(n => n.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);

            access.Verify(n => n.Generate(
                It.IsAny<long>(),
                It.IsAny<string>()),
                Times.Never);
        }


        [Fact]
        public async Task LoginAsync_WhenPasswordIsNotCorrect_ReturnNull()
        {
            var user = new Mock<ISysUserRepository>();
            var password = new Mock<IPasswordHasher>();
            var access = new Mock<IAccessTokenGenerator>();

            var existingUser = new SysUser("admin", "hashed-password");

            user.Setup(n => n.GetByUsernameAsync(
                "admin",
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            password.Setup(n => n.Verify(
                "admin123123123",
                "hashed-password"))
                .Returns(false);

            var service = new AuthService(
                user.Object,
                password.Object,
                access.Object);

            var request = new LoginRequest
            {
                Username = "admin",
                Password = "admin123123123"
            };

            var result = await service.LoginAsync(request);

            Assert.Null(result);

            password.Verify(n => n.Verify(
                "admin123123123",
                "hashed-password"),
                Times.Once);

            access.Verify(n => n.Generate(
                It.IsAny<long>(),
                It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WhenCredentialsAreCorrect_ReturnsLoginResponse()
        {
            var user = new Mock<ISysUserRepository>();
            var password = new Mock<IPasswordHasher>();
            var access = new Mock<IAccessTokenGenerator>();

            var existingUser = new SysUser("admin", "hashed-password");

            var expiresAtUtc = DateTime.UtcNow.AddHours(1);
            var accessToken = new AccessTokenResult("fake-token", expiresAtUtc);

            user.Setup(n => n.GetByUsernameAsync(
                "admin",
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            password.Setup(n => n.Verify(
                "admin123123123",
                "hashed-password"))
                .Returns(true);

            access.Setup(n => n.Generate(
                existingUser.Id,
                existingUser.Username))
                .Returns(accessToken);

            var service = new AuthService(
                user.Object,
                password.Object,
                access.Object);

            var request = new LoginRequest
            {
                Username = "admin",
                Password = "admin123123123"
            };

            var result = await service.LoginAsync(request);

            Assert.NotNull(result);

            Assert.Equal(existingUser.Id, result.UserId);
            Assert.Equal("admin", result.Username);
            Assert.Equal("fake-token", result.AccessToken);
            Assert.Equal(expiresAtUtc, result.ExpiresAtUtc);

            password.Verify(n => n.Verify(
                "admin123123123",
                "hashed-password"),
                Times.Once);

            access.Verify(n => n.Generate(
                existingUser.Id,
                existingUser.Username),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WhenUserIsInactive_ReturnsNull()
        {
            var user = new Mock<ISysUserRepository>();
            var password = new Mock<IPasswordHasher>();
            var access = new Mock<IAccessTokenGenerator>();

            var inactiveUser = new SysUser("alice", "admin123123");
            inactiveUser.Disable();

            user.Setup(n => n.GetByUsernameAsync(
                "alice",
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(inactiveUser);

            var service = new AuthService(
                user.Object,
                password.Object,
                access.Object);

            var request = new LoginRequest
            {
                Username = "alice",
                Password = "admin123123"
            };

            var result = await service.LoginAsync(request);

            Assert.Null(result);

            password.Verify(n => n.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);

            access.Verify(n => n.Generate(
                It.IsAny<long>(),
                It.IsAny<string>()),
                Times.Never);
        }
    }
}
