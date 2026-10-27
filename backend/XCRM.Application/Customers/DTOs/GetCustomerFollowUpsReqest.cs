using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using XCRM.Application.Common.Pagination;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class GetCustomerFollowUpsReqest : PageRequest, IValidatableObject
    {
        public string? Keyword { get; init; }
        public DateTimeOffset? FromTime { get; init; }
        public DateTimeOffset? ToTime { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromTime is { } from && ToTime is { } to && from >= to)
            {
                yield return new ValidationResult(
                    "开始时间必须早于结束时间",
                    new[] { nameof(FromTime), nameof(ToTime) });
            }
        }
    }
}
