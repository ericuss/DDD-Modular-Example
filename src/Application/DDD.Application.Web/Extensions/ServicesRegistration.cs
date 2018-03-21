namespace DDD.Application.Web.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using DDD.Modules;

    public static class ServicesRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, Settings settings)
        {
            Customer.Api.Configurations.ServicesRegistration.RegisterServices(services, settings);
            Users.Api.Configurations.ServicesRegistration.RegisterServices(services, settings);

            return services;
        }
    }
}
