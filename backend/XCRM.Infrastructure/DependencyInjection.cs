using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using XCRM.Application.Common.Security;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Authentication;
using XCRM.Infrastructure.Data;
using XCRM.Infrastructure.Repositories;
using XCRM.Infrastructure.Security;

namespace XCRM.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectinString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("未找到链接字符串 DefaultConnection");

            services.AddDbContext<XCrmDbContext>(options =>
            {
                options.UseSqlServer(connectinString);
            });

            services.AddScoped<ISysUserRepository, SysUserRepository>();

            services.AddSingleton<IPasswordHasher, AspNetCorePasswordHasher>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddSingleton<IAccessTokenGenerator, JwtAccessTokenGenerator>();

            return services;
        }
    }
}
