namespace DDD.Infrastructure.Data
{
    using DDD.Infrastructure.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public static class RepositoryExtensions
    {
        public static IQueryable<TEntity> IncludeProperties<TEntity>(this IQueryable<TEntity> query, string includeProperties)
            where TEntity : AggregateRoot
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, string>> orderBy, bool? ascending = true)
            where TEntity : AggregateRoot
        {
            if (orderBy != null)
            {
                if (ascending.Value) query = query.OrderBy(orderBy);
                else query = query.OrderByDescending(orderBy).AsQueryable();
            }

            return query;
        }

        public static IQueryable<TEntity> WhereFilter<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
            where TEntity : AggregateRoot
        {
            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }

            return query;
        }

        public static Task<TEntity> FirstOrDefaultFilterAsync<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
            where TEntity : AggregateRoot
        {
            if (filter != null)
            {
                return query.FirstOrDefaultAsync(filter);
            }

            return query.FirstOrDefaultAsync();
        }
        public static TEntity FirstOrDefaultFilter<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
            where TEntity : AggregateRoot
        {
            if (filter != null)
            {
                return query.FirstOrDefault(filter);
            }

            return query.FirstOrDefault();
        }

        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, int? page, int? pageSize)
           where TEntity : AggregateRoot
        {
            if (page.HasValue && page.Value >= 0
                && pageSize.HasValue && pageSize.Value > 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query;
        }
    }
}
