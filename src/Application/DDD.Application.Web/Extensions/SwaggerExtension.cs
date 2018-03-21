namespace DDD.Application.Web.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Swagger extension
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Add swaggerto services
        /// </summary>
        /// <param name="services">service collection</param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Infomed API", Version = "v1" });
            });
        }

        /// <summary>
        /// Add middlewares of swagger to application builder
        /// </summary>
        /// <param name="app">application builder</param>
        public static void UseSwaggerAndUI(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Infomed API");
            });

        }
    }
}
