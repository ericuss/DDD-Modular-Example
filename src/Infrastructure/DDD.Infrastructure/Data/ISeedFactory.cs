namespace DDD.Infrastructure.Data
{
    using System.Threading.Tasks;

    public interface ISeedFactory<TContext> where TContext : Context
    {
        Task Clean();
        Task Initialize();
    }
}
