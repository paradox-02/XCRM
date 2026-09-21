using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed record CustomerDto(long Id, string Name, string? Address, string? Remark, bool IsActive, DateTime CreateTime);
}
