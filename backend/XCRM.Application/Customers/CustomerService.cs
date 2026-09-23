using XCRM.Application.Common.Exceptions;
using XCRM.Application.Common.Pagination;
using XCRM.Application.Customers.DTOs;
using XCRM.Domain.Entities;
using XCRM.Domain.Repositories;

namespace XCRM.Application.Customers
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerContactRepository _customerContactRepository;

        public CustomerService(ICustomerRepository customerRepository, ICustomerContactRepository customerContactRepository)
        {
            _customerRepository = customerRepository;
            _customerContactRepository = customerContactRepository;
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

        public async Task<PagedResult<CustomerDto>> GetListAsync(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var keyword = string.IsNullOrWhiteSpace(request.Keyword) ? null : request.Keyword.Trim();

            var skip = (request.PageNumber - 1) * request.PageSize;

            var totalCount = await _customerRepository.CountAsync(keyword, cancellationToken);

            var customers = await _customerRepository.GetPageAsync(keyword, skip, request.PageSize, cancellationToken);

            var items = customers.Select(ToDto).ToList();

            return new PagedResult<CustomerDto>(
                items, totalCount, request.PageNumber, request.PageSize);
        }

        public async Task<CustomerDto?> UpdateDetailsAsync(long id, UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetForUpdateAsync(id, cancellationToken);

            if (customer is null)
            {
                return null;
            }

            customer.UpdateDetails(request.Address, request.Remark);

            await _customerRepository.SaveChangesAsync(cancellationToken);

            return ToDto(customer);
        }

        public async Task<CustomerDto?> UpdateStatusAsync(long id, UpdateCustomerStatusRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetForUpdateAsync(id, cancellationToken);

            if (customer is null)
            {
                return null;
            }

            if (request.IsActive == true)
            {
                customer.Enable();
            }
            else
            {
                customer.Disable();
            }

            await _customerRepository.SaveChangesAsync(cancellationToken);

            return ToDto(customer);
        }

        public async Task<CustomerContactDto?> CreateContactAsync(long customerId, CreateCustomerContactRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

            if (customer is null)
            {
                return null;
            }

            var contact = new CustomerContact(
                customerId,
                request.Name,
                request.Phone,
                request.Email);

            var exists = await _customerContactRepository.ExistsByPhoneOrEmailAsync(customer.Id, contact.Phone, contact.Email, cancellationToken);

            if (exists)
            {
                throw new ConflictException("该客户下已经存在相同的电话或者邮箱");
            }

            await _customerContactRepository.AddAsync(contact, cancellationToken);

            await _customerContactRepository.SaveChangesAsync(cancellationToken);

            return ToContactDto(contact);
        }

        private static CustomerContactDto ToContactDto(CustomerContact contact)
        {
            return new CustomerContactDto(
                contact.Id,
                contact.CustomerId,
                contact.Name,
                contact.Phone,
                contact.Email,
                contact.IsActive,
                contact.CreateTime);
        }

        public async Task<IReadOnlyList<CustomerContactDto>?> GetContactsAsync(long customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

            if (customer is null)
            {
                return null;
            }

            var contacts = await _customerContactRepository.GetByCustomerIdAsync(customerId, cancellationToken);

            return contacts.Select(ToContactDto).ToList();
        }

        public async Task<CustomerContactDto?> UpdateContactAsync(long customerId, long contactId, UpdateCustomerContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await _customerContactRepository.GetForUpdateAsync(customerId, contactId, cancellationToken);

            if (contact is null)
            {
                return null;
            }

            contact.UpdateDetails(request.Name, request.Phone, request.Email);

            var exists = await _customerContactRepository.ExistsByPhoneOrEmailExceptIdAsync(customerId, contactId, contact.Phone, contact.Email, cancellationToken);

            if (exists)
            {
                throw new ConflictException("该客户下已经存在相同的电话和邮箱");
            }

            await _customerContactRepository.SaveChangesAsync(cancellationToken);
            return ToContactDto(contact);
        }
    }
}
