using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> FindAsync(string sql, object id);

        Task<int> CountAsync(string sql, object parameter = null);

        Task<bool> AnyAsync(string sql, object parameter = null);

        Task<TEntity> GetAsync(string sql, object parameter = null);

        Task<IEnumerable<TEntity>> GetListAsync(string sql, object parameter = null);

        Task<int> AddAsync(string sql, object parameter = null);

        Task<int> UpdateAsync(string sql, object parameter = null);

        Task<int> DeleteAsync(string sql, object parameter = null);
    }
}
