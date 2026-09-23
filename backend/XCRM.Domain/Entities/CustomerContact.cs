using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Common;

namespace XCRM.Domain.Entities
{
    public class CustomerContact
    {
        public long Id { get; private set; }
        public long CustomerId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }

        public CustomerContact(long customerId, string name, string? phone, string? email)
        {
            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CustomerId = customerId;
            Name = name.Trim();
            Phone = TextNormalizer.NormalizeOptional(phone);
            Email = TextNormalizer.NormalizeOptional(email);
            IsActive = true;
            CreateTime = DateTime.UtcNow;
        }

        public void Disable()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
            UpdateTime = DateTime.UtcNow;
        }

        public void Enable()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            UpdateTime = DateTime.UtcNow;
        }

        public void UpdateDetails(string name, string? phone, string? email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            Phone = TextNormalizer.NormalizeOptional(phone);
            Email = TextNormalizer.NormalizeOptional(email);
            UpdateTime = DateTime.UtcNow;
        }
    }
}
