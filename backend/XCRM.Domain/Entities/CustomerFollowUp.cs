using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Common;

namespace XCRM.Domain.Entities
{
    public class CustomerFollowUp
    {
        public long Id { get; private set; }
        public long CustomerId { get; private set; }
        public long? ContactId { get; private set; }
        public string? PhoneSnapshot { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }

        public CustomerFollowUp(long customerId, string content, long? contactId, string? phoneSnapshot)
        {
            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(content);

            CustomerId = customerId;
            ContactId = contactId;
            PhoneSnapshot = TextNormalizer.NormalizeOptional(phoneSnapshot);
            Content = content.Trim();
            CreateTime = DateTime.UtcNow;
        }
    }
}
