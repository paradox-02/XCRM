using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed record CustomerContactDto(
        long Id,
        long CustomerId,
        string Name,
        string? Phone,
        string? Email,
        bool IsActive,
        DateTime CreateTime);
}
