using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Users.DTOs;

namespace XCRM.Application.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

        Task<UserDto?> UpdateProfileAsync(long userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
    }
}
