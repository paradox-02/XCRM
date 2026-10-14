using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using XCRM.Application.Common.Pagination;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class GetCustomerRequest : PageRequest
    {
        public string? Keyword { get; init; }
    }
}
