namespace DDD.Infrastructure.Data
{
    using DDD.Infrastructure.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class Repository<TEntity> : IRepository<TEntity>
         where TEntity : AggregateRoot
    {
        public Repository(Context context)
        {
            this.Context = context;
            this.Set = context.Set<TEntity>();
            this.Query = this.Set;
        }

        protected Context Context { get; }

        protected DbSet<TEntity> Set { get; }

        protected IQueryable<TEntity> Query { get; set; }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                return this.Query.AnyAsync(filter);
            }
            return this.Query.AnyAsync();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                return this.Query.Any(filter);
            }

            return this.Query.Any();
        }

        public virtual Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, string>> orderBy = null,
            bool? orderAscending = true,
            string includes = null,
            int? page = null,
            int? pageSize = null)
        {

            return this.Query
                   .WhereFilter(filter)
                   .IncludeProperties(includes)
                   .OrderBy(orderBy, orderAscending)
                   .Paginate(page, pageSize)
                   .ToListAsync()
                   ;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, string>> orderBy = null,
            bool? orderAscending = true,
            string includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return this.Query
                   .WhereFilter(filter)
                   .IncludeProperties(includes)
                   .OrderBy(orderBy, orderAscending)
                   .Paginate(page, pageSize)
                   .AsEnumerable();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return this.Query
                    .WhereFilter(filter)
                    .CountAsync();
        }

        public virtual Task<TEntity> GetByIdAsync(long id, string includes = null)
        {
            return this.Query
                     .IncludeProperties(includes)
                     .FirstOrDefaultAsync(t => t.Id.Equals(id))
                     ;
        }
        public virtual Task<List<TEntity>> GetByIdsAsync(List<long> ids, string includes = null)
        {
            return this.Query
                     .IncludeProperties(includes)
                     .Where(t => ids.Contains(t.Id))
                     .ToListAsync()
                     ;
        }

        public virtual TEntity GetById(long id, string includes = null)
        {
            return this.Query
                    .IncludeProperties(includes)
                    .FirstOrDefault(t => t.Id.Equals(id));
        }

        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, string includes = null)
        {
            return this.Query
                    .IncludeProperties(includes)
                    .FirstOrDefaultFilterAsync(filter)
                    ;
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string includes = null)
        {
            return this.Query
                .IncludeProperties(includes)
                .FirstOrDefaultFilter(filter)
                ;
        }

        public virtual TEntity Create(TEntity entity)
        {
            this.Set.Add(entity);
            return entity;
        }
        public virtual Task<EntityEntry<TEntity>> CreateAsync(TEntity entity)
        {
            return this.Set.AddAsync(entity);
        }

        public virtual async Task RemoveAsync(long id)
        {
            var item = await this.GetByIdAsync(id);
            this.Remove(item);
        }
        public virtual async Task RemoveAsync(List<long> ids)
        {
            var items = await this.GetByIdsAsync(ids);
            this.Remove(items);
        }

        public virtual void Remove(long id)
        {
            var item = this.GetById(id);
            this.Remove(item);
        }

        public async virtual Task Clean()
        {
            var items = await this.GetAsync();
            if (items.Any())
            {
                this.Remove(items);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.Set.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.Set.Attach(entityToDelete);
            }

            this.Set.Remove(entityToDelete);
        }

        public void Remove(List<TEntity> entitiesToRemove)
        {
            foreach (var entityToDelete in entitiesToRemove)
            {
                this.Remove(entityToDelete);
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
