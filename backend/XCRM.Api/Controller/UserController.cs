using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCRM.Application.Common.Identity;
using XCRM.Application.Users;
using XCRM.Application.Users.DTOs;

namespace XCRM.Api.Controller
{
    [ApiController]
    [Route("api/Users")]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public UserController(IUserService userService, ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
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
        public async Task<ActionResult<UserDto>> Create(CreateUserRequest request, CancellationToken cancellationToken)
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
        public async Task<ActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is not long userId)
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
