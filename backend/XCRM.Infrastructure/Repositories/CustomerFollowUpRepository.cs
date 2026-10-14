using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Common.Pagination;
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

        private IQueryable<CustomerFollowUp> BuildQuery(long customerId)
        {
            return _context.CustomerFollowUps.AsNoTracking().Where(n => n.CustomerId == customerId);
        }

        public async Task<int> CountAsync(long customerId, CancellationToken cancellationToken)
        {
            return await BuildQuery(customerId).CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CustomerFollowUp>> GetPageAsync(long customerId, int skip, int take, CancellationToken cancellationToken)
        {
            var query = BuildQuery(customerId);

            var result = await query
                .OrderByDescending(n => n.CreateTime)
                .ThenByDescending(n => n.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<CustomerFollowUp?> GetByIdAsync(long customerId, long followUpId, CancellationToken cancellationToken)
        {
            return await BuildQuery(customerId)
                .FirstOrDefaultAsync(n => n.Id == followUpId, cancellationToken);
        }
    }
}
