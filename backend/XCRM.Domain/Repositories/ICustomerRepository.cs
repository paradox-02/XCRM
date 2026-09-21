using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;

namespace XCRM.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
