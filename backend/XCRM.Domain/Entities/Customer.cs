using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Common;

namespace XCRM.Domain.Entities
{
    public class Customer
    {
        public long Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Address { get; private set; }
        public string? Remark { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }

        private Customer()
        {

        }

        public Customer(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
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

        public void UpdateDetails(string? address, string? remark)
        {
            Address = TextNormalizer.NormalizeOptional(address);
            Remark = TextNormalizer.NormalizeOptional(remark);
            UpdateTime = DateTime.UtcNow;
        }
    }
}
