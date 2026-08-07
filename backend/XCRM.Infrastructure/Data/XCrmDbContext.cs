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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SysUser>(n =>
            {
                n.HasKey(x => x.Id);

                n.Property(x => x.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                n.HasIndex(x => x.Username)
                    .IsUnique();

                n.Property(x => x.PasswordHash)
                    .HasMaxLength(500)
                    .IsRequired();

                n.Property(x => x.Email)
                    .HasMaxLength(100);

                n.Property(x => x.Phone)
                    .HasMaxLength(20);
            });
        }
    }
}
