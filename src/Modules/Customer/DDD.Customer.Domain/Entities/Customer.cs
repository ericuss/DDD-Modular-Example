namespace DDD.Customer.Domain.Entities
{
    using DDD.Infrastructure.Entities;

    public class Customer : AggregateRoot
    {
        public Customer()
        {
        }

        public Customer(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
