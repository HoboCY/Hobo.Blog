using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;

namespace Blog.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly IDbHelper<TEntity> _dbHelper;

        public Repository(IDbHelper<TEntity> dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<TEntity> FindAsync(string sql, object id)
        {
            return await _dbHelper.GetAsync(sql, new { id });
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

        public async Task<TEntity> GetAsync(string sql, object parameter = null)
        {
            return await _dbHelper.GetAsync(sql, parameter);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(string sql, object parameter = null)
        {
            return await _dbHelper.GetListAsync(sql, parameter);
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
    }
}
