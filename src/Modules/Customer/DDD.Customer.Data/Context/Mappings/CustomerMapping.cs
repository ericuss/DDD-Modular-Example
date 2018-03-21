namespace DDD.Customer.Data.Context.Mappings
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using DDD.Infrastructure.Data;
    using DDD.Customer.Domain.Entities;

    public class CustomerMapping : EntityMappingConfiguration<Customer>
    {
        public static CustomerMapping Create() => new CustomerMapping();

        public override void Map(EntityTypeBuilder<Customer> entity)
        {
            entity.ToTable("Customers");
            entity.HasKey(x => x.Id);
        }
    }
}