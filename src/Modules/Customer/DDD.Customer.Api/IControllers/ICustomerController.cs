namespace DDD.Customer.Api.IControllers
{
    using DDD.Infrastructure.Controllers;
    using DDD.Customer.Domain.Entities;

    public interface ICustomerController : IBaseApiController<Customer>
    {
    }
}
