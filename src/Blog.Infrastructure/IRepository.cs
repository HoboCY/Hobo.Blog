using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public interface IRepository<TEntity, in TKey> : IRepository<TEntity>
    {
        Task<TEntity> FindAsync(TKey id);
    }

    public interface IRepository<TEntity>
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool hasFilter = true);

        Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, bool hasFilter = true);

        Task<List<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, PagedRequest request, Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);
    }
}
