using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Authentication.DTOs;
using XCRM.Application.Common.Security;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Authentication
{
    public sealed class AuthService : IAuthService
    {
        private readonly ISysUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public AuthService(ISysUserRepository userRepository, IPasswordHasher passwordHasher, IAccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var username = request.Username.Trim();

            var user = await _userRepository.GetByUsernameAsync(username, cancellationToken);

            if (user is null || !user.IsActive)
            {
                return null;
            }

            var passwordCorrect = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordCorrect)
            {
                return null;
            }

            var accessToken = _accessTokenGenerator.Generate(user.Id, user.Username);

            return new LoginResponse(user.Id, user.Username, accessToken.Value, accessToken.ExpiresAtUtc);
        }
    }
}
