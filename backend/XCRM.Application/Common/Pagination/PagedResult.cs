using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Common.Pagination
{
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int PageNumber, int PageSize)
    {
        public PagedResult<TResult> Map<TResult>(Func<T, TResult> convert)
        {
            var result = new List<TResult>();
            foreach (var item in Items)
            {
                result.Add(convert(item));
            }
            return new PagedResult<TResult>(result, TotalCount, PageNumber, PageSize);
        }
    }
}
