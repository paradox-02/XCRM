using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed class CreateCustomerRequest
    {
        [Required(ErrorMessage = "客户名称不能为空")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "客户名称长度必须在 2 到 200 个字符之间")]
        public string Name { get; init; } = string.Empty;
    }
}
