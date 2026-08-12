using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Users.DTOs
{
    public sealed record UpdateUserProfileRequest(string? Email, string? Phone);
}
