using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Users.DTOs
{
    public sealed record UserDto(
        long Id,
        string Username,
        string? Email,
        string? Phone,
        bool IsActive,
        DateTime CreateTime);
}
