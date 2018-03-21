namespace DDD.Users.Api.Configurations
{
    using DDD.Modules;
    using DDD.Users.Api.Controllers;
    using DDD.Users.Api.IControllers;
    using DDD.Users.Data.Context;
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
            services.AddScoped<IAccountsController, AccountsController>();
            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection RegisterDB(this IServiceCollection services, Settings settings)
        {
            services.AddScoped<UsersSeedFactory>();

            UsersDbContextInitializer.Instance.ConfigureDB(services,
                                                    settings.ConnectionStrings.Customer,
                                                    "customerDb",
                                                    settings
                                                    ).Wait();
            return services;
        }
    }
}
