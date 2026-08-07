using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XCRM.Application.Authentication.DTOs
{
    public sealed class LoginRequest
    {
        [Required]
        public string Username { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
    }
}
