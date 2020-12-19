using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public interface IRepository<TEntity, TKey>
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> GetAsync(TKey key);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false);

        Task<List<TEntity>> GetListAsync(bool asNoTracking = false);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(TKey key);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    }

    public interface IRepository<TEntity>
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> GetAsync(object key);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false);

        Task<List<TEntity>> GetListAsync(bool asNoTracking = false);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(object key);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
