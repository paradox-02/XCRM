using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Domain.Repositories
{
    public sealed record CustomerFollowUpFilter(string? Keyword, DateTime? FromTimeUtc, DateTime? ToTimeUtc);
}
