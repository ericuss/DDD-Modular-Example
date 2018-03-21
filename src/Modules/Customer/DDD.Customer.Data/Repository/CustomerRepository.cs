namespace DDD.Customer.Data.Repository
{
    using DDD.Infrastructure.Data;
    using DDD.Customer.Domain.Entities;
    using DDD.Customer.Domain.IRepository;
    using DDD.Customer.Data.Context;

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerDbContext context) : base(context)
        {
        }
    }
}
