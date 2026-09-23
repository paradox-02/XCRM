using System;
using System.Collections.Generic;
using System.Text;
using XCRM.Application.Customers.DTOs;
using XCRM.Application.Common.Pagination;

namespace XCRM.Application.Customers
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
        Task<CustomerDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<PagedResult<CustomerDto>> GetListAsync(GetCustomerRequest request, CancellationToken cancellationToken);
        Task<CustomerDto?> UpdateDetailsAsync(long id, UpdateCustomerRequest request, CancellationToken cancellationToken);
        Task<CustomerDto?> UpdateStatusAsync(long id, UpdateCustomerStatusRequest request, CancellationToken cancellationToken);
        Task<CustomerContactDto?> CreateContactAsync(long customerId, CreateCustomerContactRequest request, CancellationToken cancellationToken);
        Task<IReadOnlyList<CustomerContactDto>?> GetContactsAsync(long customerId, CancellationToken cancellationToken);
    }
}
