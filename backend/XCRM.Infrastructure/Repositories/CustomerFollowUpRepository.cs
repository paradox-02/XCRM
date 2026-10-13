using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Data;

namespace XCRM.Infrastructure.Repositories
{
    public sealed class CustomerFollowUpRepository : ICustomerFollowUpRepository
    {
        private readonly XCrmDbContext _context;

        public CustomerFollowUpRepository(XCrmDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerFollowUp customerFollowUp, CancellationToken cancellationToken)
        {
            await _context.CustomerFollowUps.AddAsync(customerFollowUp, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
