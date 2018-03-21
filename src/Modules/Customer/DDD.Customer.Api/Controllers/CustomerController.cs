namespace DDD.Customer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DDD.Infrastructure.Controllers;
    using DDD.Customer.Api.IControllers;
    using DDD.Customer.Domain.Entities;
    using static DDD.Customer.Api.Configurations.Constants;
    using DDD.Customer.Domain.IRepository;

    [Route(UrlApiCustomer)]
    public class CustomerController : BaseApiController<Customer>, ICustomerController
    {
        public CustomerController(ICustomerRepository repository) : base(repository)
        {
        }

        [HttpGet("Values")]
        public IActionResult GetValues()
        {
            return this.Ok(new[] { "value1", "value2" });
        }
    }
}
