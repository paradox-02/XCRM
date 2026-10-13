using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class CreateCustomerFollowUpRequest
    {
        [Required]
        [StringLength(1000, ErrorMessage = "内容长度不能超过 1000 个字符")]
        public string Content { get; init; } = string.Empty;
        public long? ContactId { get; init; }
        [StringLength(20, ErrorMessage = "联系电话不能超过 20 个字符")]
        public string? PhoneSnapshot { get; init; }
    }
}
