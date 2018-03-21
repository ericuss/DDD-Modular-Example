﻿namespace DDD.Application.Web
{
    using DDD.Application.Web.Extensions;
    using DDD.Modules;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see 
                // http://go.microsoft.com/fwlink/?LinkID=532709
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (appAssembly != null)
                {
                    configBuilder.AddUserSecrets(appAssembly, optional: true);
                }
            }

            this.Configuration = configBuilder.Build();
            this.Settings = this.Configuration.Get<Settings>();
        }

        private readonly IConfiguration Configuration;

        private readonly Settings Settings;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // register custom services, modules, ...
            services.RegisterServices(this.Settings);

            // add mvc
            var mvc = services.AddMvc();

            // add assembly of modules for xunit
            ModuleRegistrations.RegisterModules(mvc, this.Settings);

            // add modules
            mvc.AddControllersAsServices();

            // add swagger
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerAndUI();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
