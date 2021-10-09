using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Exceptions;
using Blog.Shared;
using Dapper;
using Dapper.Transaction;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Blog.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(BlogConstants.ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), BlogConstants.InvalidConnectionString);
            _connectionString = connectionString;
        }

        public async Task<TEntity> FindAsync<TEntity, TKey>(string sql, TKey id) where TEntity : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<TEntity>(sql, new { id });
        }

        public async Task<int> CountAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(sql, parameter);
        }

        public async Task<string> GenerateIdAsync()
        {
            await using var conn = new MySqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<string>(SqlConstants.GenerateId);
        }

        public async Task<bool> AnyAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var result = await conn.ExecuteScalarAsync<int>(sql, parameter);
            return result > 0;
        }

        public async Task<TEntity> GetAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<TEntity>(sql, parameter);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            var entities = await conn.QueryAsync<TEntity>(sql, parameter);
            return entities;
        }

        public async Task InsertAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var result = await conn.ExecuteAsync(sql, parameter);
            if (result <= 0) throw new BlogException(500, BlogConstants.CreationError);
        }

        public async Task InsertManyAsync(string sql, object parameters = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                var result = await transaction.ExecuteAsync(sql, parameters);
                if (result <= 0) throw new BlogException(500, BlogConstants.CreationError);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new BlogException(500, BlogConstants.CreationError, e);
            }
        }

        public async Task UpdateAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var result = await conn.ExecuteAsync(sql, parameter);
            if (result <= 0) throw new BlogException(500, BlogConstants.UpdateError);
        }

        public async Task UpdateManyAsync(string sql, object parameters = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                var result = await transaction.ExecuteAsync(sql, parameters);
                if (result <= 0) throw new BlogException(500, BlogConstants.UpdateError);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new BlogException(500, BlogConstants.UpdateError, e);
            }
        }

        public async Task DeleteAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var result = await conn.ExecuteAsync(sql, parameter);
            if (result <= 0) throw new BlogException(500, BlogConstants.DeletionError);
        }

        public async Task DeleteManyAsync(string sql, object parameters = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                var result = await transaction.ExecuteAsync(sql, parameters);
                if (result <= 0) throw new BlogException(500, BlogConstants.DeletionError);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new BlogException(500, BlogConstants.DeletionError, e);
            }
        }

        public async Task ExecuteAsync(Dictionary<string, object> commands)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                foreach (var (key, value) in commands)
                {
                    await transaction.ExecuteAsync(key, value);
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new BlogException(500, "操作执行失败", e);
            }
        }
    }
}
