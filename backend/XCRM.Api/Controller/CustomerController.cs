using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
