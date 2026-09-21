using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Customers.DTOs;

namespace XCRM.Application.Customers
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
        Task<CustomerDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    }
}
