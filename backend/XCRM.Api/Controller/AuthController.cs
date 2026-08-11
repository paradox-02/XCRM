using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCRM.Application.Authentication;
using XCRM.Application.Authentication.DTOs;

namespace XCRM.Api.Controller
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);

            if (result is null)
            {
                return Unauthorized(new
                {
                    message = "用户名或密码错误，或账号已停用"
                });
            }

            return Ok(result);
        }                                        
    }
}
