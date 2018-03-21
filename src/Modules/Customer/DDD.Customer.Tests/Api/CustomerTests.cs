
namespace DDD.Customer.Tests.Api
{
    using Xunit;
    using Customer.Domain.Entities;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class CustomerTests : BaseIntegrationTests
    {

        [Fact]
        public async Task Values_Happy()
        {
            var request = "/api/customerModule/customer/values";
            var response = await _client.GetAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Insert_Customer()
        {
            var request = "/api/customerModule/customer";
            var customer = new Customer()
            {
                Name = "Blabla",
                Surname = "Blalba"
            };

            var response = await this.PostAsync(request, customer);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var customerInserted = JsonConvert.DeserializeObject<Customer>(responseJson);

            Assert.NotEqual(0, customerInserted.Id);
            Assert.Equal(customer.Name, customerInserted.Name);
            Assert.Equal(customer.Surname, customerInserted.Surname);
        }
    }
}
