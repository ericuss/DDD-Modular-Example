namespace DDD.Infrastructure.Data
{
    using System.Threading.Tasks;

    public abstract class SeedFactory<TContext> : ISeedFactory<TContext> 
        where TContext : Context
    {
        public abstract Task Clean();
        public abstract Task Initialize();
    }
}
