using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Data;

namespace XCRM.Infrastructure.Repositories
{
    public sealed class SysUserRepository :IsysUserRepository
    {
        private readonly XCrmDbContext _dbContext;

        public SysUserRepository(XCrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<SysUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<SysUser>().SingleOrDefaultAsync(user => user.Username == username, cancellationToken);
        }

        public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<SysUser>().AnyAsync(user => user.Username == username, cancellationToken);
        }

        public async Task AddAsync(SysUser user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<SysUser>().AddAsync(user, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
