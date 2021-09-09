using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public interface IRepository
    {
        Task<TEntity> FindAsync<TEntity>(string sql, object id) where TEntity : class, new();

        Task<int> CountAsync(string sql, object parameter = null);

        Task<bool> AnyAsync(string sql, object parameter = null);

        Task<TEntity> GetAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new();

        Task<int> AddAsync(string sql, object parameter = null);

        Task<int> UpdateAsync(string sql, object parameter = null);

        Task<int> DeleteAsync(string sql, object parameter = null);

        Task<int> ExecuteAsync(Dictionary<string, object> commands);
    }
}
