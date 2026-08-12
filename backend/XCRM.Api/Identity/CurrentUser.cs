using System.Security.Claims;
using XCRM.Application.Common.Identity;

namespace XCRM.Api.Identity
{
    internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor):ICurrentUser
    {
        private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

        public long? UserId
        {
            get
            {
                var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);

                return long.TryParse(value, out var userId) ? userId : null;
            }
        }

        public string? Username => User?.FindFirstValue(ClaimTypes.Name);
    }
}
