using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Domain.Entities;

namespace XCRM.Infrastructure.Data
{
    public class XCrmDbContext : DbContext
    {
        public XCrmDbContext(DbContextOptions<XCrmDbContext> options) : base(options)
        {

        }

        public DbSet<SysUser> Users => Set<SysUser>();
    }
}
