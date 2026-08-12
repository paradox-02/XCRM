using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XCRM.Application.Users;
using XCRM.Application.Users.DTOs;

namespace XCRM.Api.Controller
{
    [ApiController]
    [Route("api/Users")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<ActionResult<UserDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserRequest request,CancellationToken cancellationToken)
        {
            var user = await _userService.CreateAsync(request, cancellationToken);

            if (user is null)
            {
                return Conflict(new
                {
                    message = "用户名已经存在"
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult GetCurrentUser()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var username = User.FindFirstValue(ClaimTypes.Name);

            if (!long.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                userId,
                username
            });
        }
    }
}
