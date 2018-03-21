namespace DDD.Infrastructure.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DDD.Infrastructure.Entities;
    using DDD.Infrastructure.Data;
    using System.Threading.Tasks;

    public abstract class BaseApiController<TEntity> : BaseEmptyApiController, IBaseApiController<TEntity>
        where TEntity : AggregateRoot
    {
        protected readonly IRepository<TEntity> repository;

        public BaseApiController(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var collection = await this.repository.GetAsync();
            return this.Ok(collection);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]TEntity entity)
        {
            if (entity != null)
            {
                var resultRaw = await this.repository.CreateAsync(entity);
                var isSaved = await this.repository.SaveChangesAsync();
                if (isSaved > 0)
                {
                    return this.Ok((TEntity)resultRaw.Entity);
                }
            }

            return this.BadRequest();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] TEntity entity)
        {
            if (id > 0)
            {
                this.repository.Update(entity);
                var isSaved = await this.repository.SaveChangesAsync();
                if (isSaved > 0)
                {
                    return this.Ok(entity);
                }
            }

            return this.BadRequest();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await this.repository.RemoveAsync(id);
                var isSaved = await this.repository.SaveChangesAsync();
                if (isSaved > 0)
                {
                    return this.Ok(new { response = "Ok" });
                }
            }

            return this.BadRequest();
        }
    }





}
