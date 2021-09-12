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

        Task<string> GenerateIdAsync();

        Task<bool> AnyAsync(string sql, object parameter = null);

        Task<TEntity> GetAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new();

        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new();

        Task InsertAsync(string sql, object parameter = null);

        Task InsertManyAsync(string sql, object parameters = null);

        Task UpdateAsync(string sql, object parameter = null);

        Task UpdateManyAsync(string sql, object parameters = null);

        Task DeleteAsync(string sql, object parameter = null);

        Task DeleteManyAsync(string sql, object parameters = null);

        Task ExecuteAsync(Dictionary<string, object> commands);
    }
}
