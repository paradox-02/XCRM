using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Common.Security;

namespace XCRM.Infrastructure.Security
{
    public sealed class AspNetCorePasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        private readonly object _passwordOwner = new();

        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(_passwordOwner, password);
        }

        public bool Verify(string password, string passwordHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(_passwordOwner, passwordHash, password);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
