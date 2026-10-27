using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;

namespace XCRM.Domain.Repositories
{
    public interface ICustomerFollowUpRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task AddAsync(CustomerFollowUp customerFollowUp, CancellationToken cancellationToken = default);
        Task<int> CountAsync(long customerId, CustomerFollowUpFilter filter, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CustomerFollowUp>> GetPageAsync(long customerId, CustomerFollowUpFilter filter, int skip, int take, CancellationToken cancellationToken = default);
        Task<CustomerFollowUp?> GetByIdAsync(long customerId, long followUpId, CancellationToken cancellationToken = default);
    }
}
