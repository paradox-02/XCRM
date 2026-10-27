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

        private IQueryable<CustomerFollowUp> BuildQuery(long customerId, CustomerFollowUpFilter? filter = null)
        {
            var query = _context.CustomerFollowUps.AsNoTracking().Where(n => n.CustomerId == customerId);

            if (filter is null)
            {
                return query;
            }

            if (filter.Keyword is { } keyword)
            {
                query = query.Where(n => n.Content.Contains(keyword));
            }

            if (filter.FromTimeUtc is { } from)
            {
                query = query.Where(n => n.CreateTime >= from);
            }


            return query;
        }

        public async Task<int> CountAsync(long customerId, CustomerFollowUpFilter filter, CancellationToken cancellationToken)
        {
            return await BuildQuery(customerId, filter).CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CustomerFollowUp>> GetPageAsync(long customerId, CustomerFollowUpFilter filter, int skip, int take, CancellationToken cancellationToken)
        {
            var query = BuildQuery(customerId, filter);

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
