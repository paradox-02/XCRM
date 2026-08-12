using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Common.Identity
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        long? UserId { get; }
        string? Username { get; }
    }
}
