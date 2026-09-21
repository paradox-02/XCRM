using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class UpdateCustomerRequest
    {
        [StringLength(500)]
        public string? Address { get; init; }
        [StringLength(1000)]
        public string? Remark { get; init; }
    }
}
