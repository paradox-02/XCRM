using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Users.DTOs
{
    public sealed record UpdateUserProfileRequest
    {
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        [StringLength(100, ErrorMessage = "邮箱不能超过 100 个字符")]
        public string? Email { get; init; }

        [StringLength(20, ErrorMessage = "手机号不能超过 20 个字符")]
        public string? Phone { get; init; }
    }
}
