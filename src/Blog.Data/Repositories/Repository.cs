using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;

namespace Blog.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly IDbHelper _dbHelper;

        public Repository(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<TEntity> FindAsync<TEntity>(string sql, object id) where TEntity : class, new()
        {
            return await _dbHelper.GetAsync<TEntity>(sql, new { id });
        }

        public async Task<int> CountAsync(string sql, object parameter = null)
        {
            var result = await _dbHelper.GetScalarAsync(sql, parameter);
            return Convert.ToInt32(result);
        }

        public async Task<bool> AnyAsync(string sql, object parameter = null)
        {
            var result = await _dbHelper.GetScalarAsync(sql, parameter);
            return result == null;
        }

        public async Task<TEntity> GetAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            return await _dbHelper.GetAsync<TEntity>(sql, parameter);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            return await _dbHelper.GetListAsync<TEntity>(sql, parameter);
        }

        public async Task<int> AddAsync(string sql, object parameter = null)
        {
            return await _dbHelper.ExecuteAsync(sql, parameter);
        }

        public async Task<int> UpdateAsync(string sql, object parameter = null)
        {
            return await _dbHelper.ExecuteAsync(sql, parameter);
        }

        public async Task<int> DeleteAsync(string sql, object parameter = null)
        {
            return await _dbHelper.ExecuteAsync(sql, parameter);
        }

        public async Task<int> ExecuteAsync(Dictionary<string, object> commands)
        {
            return await _dbHelper.ExecuteAsync(commands);
        }
    }
}
