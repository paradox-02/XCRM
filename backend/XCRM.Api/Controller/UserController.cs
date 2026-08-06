using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UserDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }
    }
}
