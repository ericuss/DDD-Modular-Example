namespace DDD.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    public abstract class Context : DbContext
    {

        protected Context(DbContextOptions options) : base(options)
        {
        }
    }
}
