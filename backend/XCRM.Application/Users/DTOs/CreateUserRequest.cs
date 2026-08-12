using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Users.DTOs
{
    public sealed class CreateUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; init; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; init; } = string.Empty;
    }
}
