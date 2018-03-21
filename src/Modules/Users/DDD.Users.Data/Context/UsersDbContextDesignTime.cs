using DDD.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Users.Data.Context
{
    public class UsersDbContextDesignTime : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            var settings = config.Get<Settings>();

            var builder = new DbContextOptionsBuilder<UsersDbContext>();

            var connectionString = settings.ConnectionStrings.Users;

            builder.UseSqlServer(connectionString);

            return new UsersDbContext(builder.Options);
        }
    }
}
