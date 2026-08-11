using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XCRM.Application.Common.Security;

namespace XCRM.Infrastructure.Authentication
{
    public sealed class JwtAccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly JwtOptions _options;

        public JwtAccessTokenGenerator(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AccessTokenResult Generate(long userId, string username)
        {
            var now = DateTime.UtcNow;
            var expiresAtUtc = now.AddMinutes(_options.ExpirationMinutes);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),

                new Claim(ClaimTypes.Name,username),

                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var keyBytes = Convert.FromBase64String(_options.Key);

            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new AccessTokenResult(tokenValue, expiresAtUtc);
        }
    }
}
