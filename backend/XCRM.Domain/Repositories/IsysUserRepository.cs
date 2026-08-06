using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;

namespace XCRM.Domain.Repositories
{
    public interface IsysUserRepository
    {
        Task<SysUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task AddAsync(SysUser user, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<SysUser?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}
