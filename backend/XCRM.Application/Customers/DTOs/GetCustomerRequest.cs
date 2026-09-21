using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class GetCustomerRequest
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; } = 1;
        [Range(1, 100)]
        public int PageSize { get; init; } = 20;
        public string? Keyword { get; init; }
    }
}
