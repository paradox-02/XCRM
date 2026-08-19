using Moq;
using XCRM.Application.Common.Exceptions;
using XCRM.Application.Common.Security;
using XCRM.Application.Users;
using XCRM.Application.Users.DTOs;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Tests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenUsernameExists_ThrowsConflictException()
    {
        var userRepository = new Mock<ISysUserRepository>();

        userRepository.Setup(n => n.ExistsByUsernameAsync(
            "admin",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var passwordHasher = new Mock<IPasswordHasher>();

        var service = new UserService(userRepository.Object, passwordHasher.Object);

        var request = new CreateUserRequest
        {
            Username = "admin",
            Password = "password123"
        };

        var exception = await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(request));

        Assert.Equal("用户名已经存在", exception.Message);

        passwordHasher.Verify(
            n => n.Hash(
                It.IsAny<string>()),
            Times.Never);

        userRepository.Verify(
            n => n.AddAsync(
                It.IsAny<SysUser>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        userRepository.Verify(
            n => n.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenUsernameDoesNotExist_CreatesUser()
    {
        var userRepository = new Mock<ISysUserRepository>();

        userRepository.Setup(n => n.ExistsByUsernameAsync(
            "alice",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        userRepository.Setup(n => n.AddAsync(
            It.IsAny<SysUser>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        userRepository.Setup(n => n.SaveChangesAsync(
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var passwordHasher = new Mock<IPasswordHasher>();

        passwordHasher.Setup(n => n.Hash("password123"))
            .Returns("hashed-password");

        var service = new UserService(userRepository.Object, passwordHasher.Object);

        var request = new CreateUserRequest
        {
            Username = "alice",
            Password = "password123"
        };

        var result = await service.CreateAsync(request);

        Assert.Equal("alice", result.Username);
        Assert.True(result.IsActive);

        passwordHasher.Verify(n => n.Hash("password123"), Times.Once);

        userRepository.Verify(n => n.AddAsync(
            It.Is<SysUser>(user =>
            user.Username == "alice" &&
            user.PasswordHash == "hashed-password"),
            It.IsAny<CancellationToken>()),
            Times.Once);

        userRepository.Verify(n => n.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
