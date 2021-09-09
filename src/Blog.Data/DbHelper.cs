﻿using System;
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
    public class DbHelper<TEntity> : IDbHelper<TEntity> where TEntity : class, new()
    {
        private readonly MySqlConnection _connection;

        public DbHelper(IConfiguration configuration, MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> ExecuteAsync(string sql, object parameter = null)
        {
            await _connection.OpenAsync();
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> ExecuteAsync(Dictionary<string, object> commands)
        {
            try
            {
                await _connection.OpenAsync();
                await using var transaction = await _connection.BeginTransactionAsync();
                using var batch = _connection.CreateBatch();
                batch.Transaction = transaction;
                foreach (var (key, value) in commands)
                {
                    var command = new MySqlBatchCommand(key);
                    command.SetParameters(value);
                    batch.BatchCommands.Add(command);
                }
                await batch.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<TEntity> GetAsync(string sql, object parameter = null)
        {
            await _connection.OpenAsync();
            await using var cmd = _connection.CreateCommand();
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
            await _connection.OpenAsync();

            await using var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.SetParameters(parameter);

            return await cmd.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(string sql, object parameter = null)
        {
            await _connection.OpenAsync();

            await using var cmd = _connection.CreateCommand();
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
