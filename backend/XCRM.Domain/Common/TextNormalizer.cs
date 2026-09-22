using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Domain.Common
{
    internal static class TextNormalizer
    {
        public static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
