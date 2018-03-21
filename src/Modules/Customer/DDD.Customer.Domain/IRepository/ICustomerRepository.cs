namespace DDD.Customer.Domain.IRepository
{
    using DDD.Infrastructure.Data;
    using DDD.Customer.Domain.Entities;

    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}
