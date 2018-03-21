namespace DDD.Customer.Data.Context
{
    using DDD.Infrastructure.Data;

    public class CustomerDbContextInitializer : ContextInitializer<CustomerDbContext>
    {
        public static CustomerDbContextInitializer Instance => new CustomerDbContextInitializer();
    }
}
