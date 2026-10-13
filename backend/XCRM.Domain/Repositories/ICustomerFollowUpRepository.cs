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
    }
}
