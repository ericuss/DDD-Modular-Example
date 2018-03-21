namespace DDD.Customer.Api.Configurations
{
    using DDD.Customer.Api.Controllers;
    using DDD.Customer.Api.IControllers;
    using DDD.Customer.Data.Context;
    using DDD.Customer.Data.Repository;
    using DDD.Customer.Domain.IRepository;
    using DDD.Infrastructure.Data;
    using DDD.Modules;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesRegistration
    {
        public static IServiceCollection RegisterServices(IServiceCollection services, Settings settings)
        {
            services
                .RegisterApis()
                .RegisterRepositories()
                .RegisterDB(settings)
                ;

            return services;
        }

        private static IServiceCollection RegisterApis(this IServiceCollection services)
        {
            services.AddScoped<ICustomerController, CustomerController>();
            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }

        private static IServiceCollection RegisterDB(this IServiceCollection services, Settings settings)
        {
            services.AddScoped<ISeedFactory<CustomerDbContext>, CustomerSeedFactory>();

            CustomerDbContextInitializer.Instance.ConfigureDB(services,
                                                    settings.ConnectionStrings.Customer,
                                                    "customerDb",
                                                    settings
                                                    ).Wait();

            return services;
        }
    }
}
