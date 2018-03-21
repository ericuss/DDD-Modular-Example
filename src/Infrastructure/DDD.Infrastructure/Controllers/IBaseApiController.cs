namespace DDD.Infrastructure.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DDD.Infrastructure.Entities;
    using DDD.Infrastructure.Data;
    using System.Threading.Tasks;

    public interface IBaseApiController<TEntity>
        where TEntity : AggregateRoot
    {
        Task<IActionResult> Get();

        Task<IActionResult> Post([FromBody]TEntity entity);

        Task<IActionResult> Put(int id, [FromBody] TEntity entity);

        Task<IActionResult> Delete(int id);
    }
}
