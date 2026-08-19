using Moq;
using XCRM.Application.Common.Exceptions;
using XCRM.Application.Common.Security;
using XCRM.Application.Users;
using XCRM.Application.Users.DTOs;
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
    }
}
