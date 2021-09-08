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

namespace Blog.Data
{
    public class DbHelper<TEntity> : IDbHelper<TEntity> where TEntity : class, new()
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(BlogConstants.ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Invalid connection string");
            _connectionString = connectionString;
        }

        public async Task<int> ExecuteAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(sql, conn);
            cmd.SetParameters(parameter);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<TEntity> GetAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(sql, conn);
            cmd.SetParameters(parameter);

            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var objType = typeof(TEntity);
            var entity = new TEntity();
            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();

            if (!reader.HasRows) throw new BlogEntityNotFoundException(typeof(TEntity), parameter);

            while (reader.Read())
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

            var cmd = new MySqlCommand(sql, conn);
            cmd.SetParameters(parameter);

            return await cmd.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new MySqlCommand(sql, conn);
            cmd.SetParameters(parameter);

            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var dataList = new List<TEntity>();

            var objType = typeof(TEntity);

            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();

            while (reader.Read())
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
