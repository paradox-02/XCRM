using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Authentication.DTOs
{
    public sealed record LoginResponse(long UserId, string Username);
}
