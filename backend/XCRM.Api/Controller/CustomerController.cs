using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCRM.Application.Common.Pagination;
using XCRM.Application.Customers;
using XCRM.Application.Customers.DTOs;

namespace XCRM.Api.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.CreateAsync(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, customer);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CustomerDto>> GetById(long id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetByIdAsync(id, cancellationToken);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CustomerDto>>> GetList([FromQuery] GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _customerService.GetListAsync(request, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<CustomerDto>> UpdateDetails(long id, UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.UpdateDetailsAsync(id, request, cancellationToken);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPut("{id:long}/status")]
        public async Task<ActionResult<CustomerDto>> UpdateStatus(long id, UpdateCustomerStatusRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.UpdateStatusAsync(id, request, cancellationToken);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost("{customerId:long}/contacts")]
        public async Task<ActionResult<CustomerContactDto>> CreateContact(long customerId, CreateCustomerContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await _customerService.CreateContactAsync(customerId, request, cancellationToken);

            if (contact is null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status201Created, contact);
        }

        [HttpGet("{customerId:long}/contacts")]
        public async Task<ActionResult<IReadOnlyList<CustomerContactDto>>> GetContacts(long customerId, CancellationToken cancellationToken)
        {
            var contacts = await _customerService.GetContactsAsync(customerId, cancellationToken);

            if (contacts is null)
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        [HttpPut("{customerId:long}/contacts/{contactId:long}")]
        public async Task<ActionResult<CustomerContactDto>> UpdateContact(long customerId, long contactId, UpdateCustomerContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await _customerService.UpdateContactAsync(customerId, contactId, request, cancellationToken);

            if (contact is null)
            {
                return NotFound();
            }

            return Ok(contact);
        }
    }
}
