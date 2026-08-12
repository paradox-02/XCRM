using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Users.DTOs
{
    public sealed class CreateUserRequest
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在 3 到 50 个字符之间")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "密码长度必须在 8 到 100 个字符之间")]
        public string Password { get; init; } = string.Empty;
    }
}
