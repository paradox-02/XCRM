using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Data;

namespace XCRM.Infrastructure.Repositories
{
    public sealed class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly XCrmDbContext _context;

        public CustomerContactRepository(XCrmDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerContact contact, CancellationToken cancellationToken = default)
        {
            await _context.CustomerContacts.AddAsync(contact, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByPhoneOrEmailAsync(long customerId, string? phone, string? email, CancellationToken cancellationToken)
        {
            if (phone is null && email is null)
            {
                return false;
            }

            return await _context.CustomerContacts.AsNoTracking().AnyAsync(n =>
            n.CustomerId == customerId &&
            (
            phone != null && n.Phone == phone ||
            email != null && n.Email == email
            ),
            cancellationToken);
        }

        public async Task<IReadOnlyList<CustomerContact>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken)
        {
            return await _context.CustomerContacts.AsNoTracking()
                .Where(n => n.CustomerId == customerId)
                .OrderByDescending(n => n.CreateTime)
                .ThenByDescending(n => n.Id)
                .ToListAsync(cancellationToken);
        }

        public Task<CustomerContact?> GetForUpdateAsync(long customerId, long contactId, CancellationToken cancellationToken)
        {
            return _context.CustomerContacts.FirstOrDefaultAsync(n => n.CustomerId == customerId && n.Id == contactId, cancellationToken);
        }

        public Task<bool> ExistsByPhoneOrEmailExceptIdAsync(long customerId, long contactId, string? phone, string? email, CancellationToken cancellationToken)
        {
            if (phone is null && email is null)
            {
                return Task.FromResult(false);
            }

            return _context.CustomerContacts.AsNoTracking()
                .AnyAsync(n => n.CustomerId == customerId && n.Id != contactId
                && ((phone != null && n.Phone == phone)
                || (email != null && n.Email == email)), cancellationToken);
        }
    }
}
