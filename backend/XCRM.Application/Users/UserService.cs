using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Users.DTOs;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Users
{
    public sealed class UserService : IUserService
    {
        private readonly IsysUserRepository _userRepository;
        
        public UserService(IsysUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user is null)
                return null;

            return new UserDto(
                user.Id,
                user.Username,
                user.Email,
                user.Phone,
                user.IsActive,
                user.CreateTime);
        }
    }
}
