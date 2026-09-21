using Microsoft.EntityFrameworkCore;
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

        private IQueryable<Customer> BuildQuery(string? keyword)
        {
            var query = _context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = keyword.Trim();

                query = query.Where(n => n.Name.Contains(normalizedKeyword));
            }

            return query;
        }

        public async Task<IReadOnlyList<Customer>> GetPageAsync(string? keyword, int skip, int take, CancellationToken cancellationToken)
        {
            return await BuildQuery(keyword)
                .OrderByDescending(n => n.CreateTime)
                .ThenByDescending(n => n.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public Task<int> CountAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            return BuildQuery(keyword).CountAsync(cancellationToken);
        }
    }
}
