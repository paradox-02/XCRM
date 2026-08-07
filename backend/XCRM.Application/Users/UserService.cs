using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using XCRM.Application.Common.Security;
using XCRM.Application.Users.DTOs;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Users
{
    public sealed class UserService : IUserService
    {
        private readonly ISysUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        
        public UserService(ISysUserRepository userRepository,IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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

        public async Task<UserDto?> CreateAsync(CreateUserRequest request,CancellationToken cancellationToken = default)
        {
            var username = request.Username.Trim();

            var exists = await _userRepository.ExistsByUsernameAsync(username, cancellationToken);

            if (exists)
                return null;

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new SysUser(username, passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);

            await _userRepository.SaveChangesAsync(cancellationToken);

            return ToDto(user);
        }

        private static UserDto ToDto(SysUser user)
        {
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
