﻿namespace DDD.Users.Data.Context
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using DDD.Modules;
    using DDD.Users.Domain.Entities;

    public class UsersDbContextInitializer
    {
        public static UsersDbContextInitializer Instance => new UsersDbContextInitializer();

        public async Task ConfigureDB(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            this.AddToDI(services, connectionString, dbName, settings);
            await this.Initialize(services, connectionString, dbName, settings);
        }

        private void AddToDI(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            if (settings.Database.UseInMemory)
            {
                services.AddDbContext<UsersDbContext>(o => o.UseInMemoryDatabase(dbName));
            }
            else
            {
                services.AddDbContext<UsersDbContext>(options =>
                  options.UseSqlServer(settings.ConnectionStrings.Customer));
            }

            this.ConfigureIdentity(services);
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Lockout settings
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders();
        }

        private async Task Initialize(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            DbContextOptionsBuilder<UsersDbContext> optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

            if (settings.Database.UseInMemory) optionsBuilder.UseInMemoryDatabase(dbName);
            else optionsBuilder.UseSqlServer(connectionString);

            var context = new UsersDbContext(optionsBuilder.Options);
            if (settings.Database.Regenerate && !settings.Database.UseInMemory)
            {
                // Service locator
                var sp = services.BuildServiceProvider();
                var seedFactory = sp.GetService<UsersSeedFactory>();

                await seedFactory.Clean();
                context.Database.Migrate();

                var userManager = sp.GetService<UserManager<ApplicationUser>>();
                await seedFactory.Initialize();
            }
            else context.Database.Migrate();
        }
    }
}
