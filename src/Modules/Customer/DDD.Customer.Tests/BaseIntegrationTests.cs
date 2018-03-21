using DDD.Infrastructure.Data;
using System;
using Xunit;

namespace DDD.Customer.Tests
{
    using Customer.Domain.Entities;
    using DDD.Application.Web;
    using DDD.Customer.Data.Repository;
    using DDD.Customer.Domain.IRepository;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class BaseIntegrationTests
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        public BaseIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        protected Task<HttpResponseMessage> PostAsync<TObject>(string request, TObject item)
        {
            var serialized = this.Serialize(item);
            return _client.PostAsync(request, serialized);
        }

        protected ByteArrayContent Serialize<TObject>(TObject data)
        {
            var serialized = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(serialized);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}
