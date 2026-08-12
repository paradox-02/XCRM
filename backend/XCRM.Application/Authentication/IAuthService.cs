using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Authentication.DTOs;

namespace XCRM.Application.Authentication
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
