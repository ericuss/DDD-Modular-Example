namespace DDD.Customer.Data.Context
{
    using Microsoft.EntityFrameworkCore;
    using DDD.Infrastructure.Data;
    using DDD.Customer.Domain.Entities;
    using DDD.Customer.Data.Context.Mappings;

    public class CustomerDbContext : Context
    {

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CustomerMapping.Create().Map(modelBuilder);
        }
    }
}
