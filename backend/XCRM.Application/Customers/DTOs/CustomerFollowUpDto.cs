using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Application.Customers.DTOs
{
    public sealed record CustomerFollowUpDto(long id, long customerId, long? contactId, string? phoneSnapshot, string content, DateTime createTime);
}
