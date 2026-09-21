using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Customers.DTOs;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Data;

namespace XCRM.Infrastructure.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly XCrmDbContext _context;

        public CustomerRepository(XCrmDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(n => n.Name == name, cancellationToken);
        }

        public Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return _context.Customers.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }
    }
}
