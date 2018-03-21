namespace DDD.Infrastructure.Data
{
    using DDD.Modules;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public class ContextInitializer<TContext>
        where TContext : Context
    {
        public async Task ConfigureDB(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            this.AddToDI(services, connectionString, dbName, settings);
            await this.Initialize(services, connectionString, dbName, settings);
        }

        private void AddToDI(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            if (settings.Database.UseInMemory)
            {
                services.AddDbContext<TContext>(o => o.UseInMemoryDatabase(dbName));
            }
            else
            {
                services.AddDbContext<TContext>(o => o.UseSqlServer(connectionString));
            }
        }

        private async Task Initialize(IServiceCollection services, string connectionString, string dbName, Settings settings)
        {
            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();

            if (settings.Database.UseInMemory) optionsBuilder.UseInMemoryDatabase(dbName);
            else optionsBuilder.UseSqlServer(connectionString);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options });
            if (settings.Database.Regenerate && !settings.Database.UseInMemory)
            {
                // Service locator
                var seedFactory = services.BuildServiceProvider().GetService<ISeedFactory<TContext>>();

                await seedFactory.Clean();
                context.Database.Migrate();
                await seedFactory.Initialize();
            }
            else context.Database.Migrate();
        }
    }
}
