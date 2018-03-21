namespace DDD.Infrastructure.Data
{
    using DDD.Infrastructure.Entities;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity>
         where TEntity : AggregateRoot
    {

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

        bool Any(Expression<Func<TEntity, bool>> filter = null);

        Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, string>> orderBy = null,
            bool? orderAscending = true,
            string includes = null,
            int? page = null,
            int? pageSize = null);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, string>> orderBy = null,
            bool? orderAscending = true,
            string includes = null,
            int? page = null,
            int? pageSize = null);

        Task Clean();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> GetByIdAsync(long id, string includes = null);

        TEntity GetById(long id, string includes = null);

        Task<List<TEntity>> GetByIdsAsync(List<long> ids, string includes = null);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, string includes = null);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string includes = null);

        TEntity Create(TEntity entity);

        Task<EntityEntry<TEntity>> CreateAsync(TEntity entity);

        void Update(TEntity entityToUpdate);

        Task RemoveAsync(long id);

        void Remove(long id);

        Task RemoveAsync(List<long> id);

        void Remove(TEntity entityToDelete);

        void Remove(List<TEntity> entitiesToRemove);

        Task<int> SaveChangesAsync();

        int SaveChanges();

    }
}
