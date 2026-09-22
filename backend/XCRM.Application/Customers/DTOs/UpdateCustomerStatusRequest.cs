using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class UpdateCustomerStatusRequest
    {
        [Required]
        public bool? IsActive { get; init; }
    }
}
