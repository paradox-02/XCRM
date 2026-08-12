using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Common.Security
{
    public sealed record AccessTokenResult(string Value, DateTime ExpiresAtUtc);
}
