using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class CreateCustomerContactRequest
    {
        [Required(ErrorMessage = "联系人姓名不能为空")]
        [StringLength(100, ErrorMessage = "联系人姓名不能超过 100 个字符")]
        public string Name { get; init; } = string.Empty;

        [StringLength(20, ErrorMessage = "联系电话不能超过 20 个字符")]
        public string? Phone { get; init; }

        [StringLength(200, ErrorMessage = "邮箱不能超过 200 个字符")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string? Email { get; init; }
    }
}
