using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;

namespace XCRM.Domain.Repositories
{
    public interface ICustomerContactRepository
    {
        Task AddAsync(CustomerContact contact, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByPhoneOrEmailAsync(long customerId, string? phone, string? email, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CustomerContact>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default);
    }
}
