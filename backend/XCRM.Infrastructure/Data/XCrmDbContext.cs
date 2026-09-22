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
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerContact> CustomerContacts => Set<CustomerContact>();

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

            modelBuilder.Entity<Customer>(n =>
            {
                n.HasKey(x => x.Id);

                n.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

                n.Property(x => x.Address)
                .HasMaxLength(500);

                n.Property(x => x.Remark)
                .HasMaxLength(1000);
            });

            modelBuilder.Entity<CustomerContact>(n =>
            {
                n.HasKey(x => x.Id);

                n.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

                n.Property(x => x.Phone)
                .HasMaxLength(20);

                n.Property(x => x.Email)
                .HasMaxLength(200);

                n.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
