using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data
{
    public interface IDbHelper<TEntity> where TEntity : class, new()
    {
        Task<int> ExecuteAsync(string sql, object parameter = null);

        Task<int> ExecuteAsync(Dictionary<string, object> commands);

        Task<TEntity> GetAsync(string sql, object parameter = null);

        Task<object> GetScalarAsync(string sql, object parameter = null);

        Task<IEnumerable<TEntity>> GetListAsync(string sql, object parameter = null);
    }
}
