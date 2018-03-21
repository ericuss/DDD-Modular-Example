namespace DDD.Customer.Data.Context
{
    using DDD.Customer.Domain.IRepository;
    using DDD.Infrastructure.Data;
    using System.Threading.Tasks;

    public class CustomerSeedFactory : SeedFactory<CustomerDbContext>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerSeedFactory(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }
        public async override Task Clean()
        {
            await this._customerRepository.Clean();

            await this._customerRepository.SaveChangesAsync();
        }

        public async override Task Initialize()
        {
            await this.AddCustomers();

            await this._customerRepository.SaveChangesAsync();
        }

        private async Task AddCustomers()
        {
            await this._customerRepository.CreateAsync(new Domain.Entities.Customer("Adam", "Smith"));
            await this._customerRepository.CreateAsync(new Domain.Entities.Customer("Lara", "Smith"));
            await this._customerRepository.CreateAsync(new Domain.Entities.Customer("Robert", "Smith"));
        }
    }
}
