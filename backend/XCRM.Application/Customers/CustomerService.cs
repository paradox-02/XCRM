using XCRM.Application.Common.Exceptions;
using XCRM.Application.Customers.DTOs;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Customers
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exist = await _customerRepository.ExistsByNameAsync(name, cancellationToken);

            if (exist)
            {
                throw new ConflictException("客户已经存在");
            }

            var customer = new Customer(name);

            await _customerRepository.AddAsync(customer, cancellationToken);

            await _customerRepository.SaveChangesAsync(cancellationToken);

            return new CustomerDto(
                customer.Id,
                customer.Name,
                customer.Address,
                customer.Remark,
                customer.IsActive,
                customer.CreateTime);
        }

        public async Task<CustomerDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

            if (customer is null)
            {
                return null;
            }

            return ToDto(customer);
        }

        private static CustomerDto ToDto(Customer customer)
        {
            return new CustomerDto(
                customer.Id,
                customer.Name,
                customer.Address,
                customer.Remark,
                customer.IsActive,
                customer.CreateTime);
        }
    }
}
