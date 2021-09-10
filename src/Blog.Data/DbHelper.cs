using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Extensions;
using Blog.Exceptions;
using Blog.Shared;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using MySqlConnector.Logging;

namespace Blog.Data
{
    public class DbHelper : IDbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(BlogConstants.ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString), "Invalid ConnectionString");
            _connectionString = connectionString;
        }

        public async Task<int> ExecuteAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> ExecuteAsync(Dictionary<string, object> commands)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                using var batch = conn.CreateBatch();
                batch.Transaction = transaction;
                foreach (var (key, value) in commands)
                {
                    var command = new MySqlBatchCommand(key);
                    command.SetParameters(value);
                    batch.BatchCommands.Add(command);
                }
                var result = await batch.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }

        public async Task<TEntity> GetAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var objType = typeof(TEntity);
            var entity = new TEntity();
            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();

            if (!reader.HasRows) throw new BlogEntityNotFoundException(typeof(TEntity), parameter);

            while (await reader.ReadAsync())
            {
                foreach (var item in properties)
                {
                    reader.SetValue<TEntity>(item, entity);
                }
            }

            return (TEntity)entity;
        }

        public async Task<object> GetScalarAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            return await cmd.ExecuteScalarAsync();
        }

        public async Task<object> GetScalarAsync<T>(string sql, List<T> parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();

            var paramName = new StringBuilder();
            for (int i = 0; i < parameters.Count; i++)
            {
                paramName.Append($"@p{i}");
                if (i < parameters.Count - 1) paramName.Append(",");
                cmd.Parameters.AddWithValue($"p{i}", parameters[i]);
            }
            cmd.CommandText = string.Format(sql, paramName);
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(string sql, object parameter = null) where TEntity : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var dataList = new List<TEntity>();

            var objType = typeof(TEntity);

            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();

            while (await reader.ReadAsync())
            {
                var entity = new TEntity();
                foreach (var item in properties)
                {
                    reader.SetValue<TEntity>(item, entity);
                }

                dataList.Add(entity);
            }

            return dataList;
        }
    }
}
