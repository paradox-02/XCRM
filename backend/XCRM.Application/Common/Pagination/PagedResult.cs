using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Common.Pagination
{
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int PageNumber, int PageSize);
}
