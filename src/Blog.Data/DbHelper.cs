using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Blog.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> ExecuteAsync(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new MySqlCommand(sql, conn);
            if (parameter != null)
            {
                var paramType = parameter.GetType();
                foreach (var item in paramType.GetProperties())
                {
                    cmd.Parameters.AddWithValue(item.Name, item.GetValue(parameter));
                }
            }

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<T> GetAsync<T>(string sql, object parameter = null) where T : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new MySqlCommand(sql, conn);
            if (parameter != null)
            {
                var paramType = parameter.GetType();
                foreach (var item in paramType.GetProperties())
                {
                    cmd.Parameters.AddWithValue(item.Name, item.GetValue(parameter));
                }
            }
            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var objType = typeof(T);
            var entity = new T();
            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();
            while (reader.Read())
            {
                foreach (var item in properties)
                {
                    SetValue(item, entity, reader);
                }
            }
            return (T)entity;
        }

        public async Task<int> GetCountAsync<T>(string sql, object parameter = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new MySqlCommand(sql, conn);
            if (parameter != null)
            {
                var paramType = parameter.GetType();
                foreach (var item in paramType.GetProperties())
                {
                    cmd.Parameters.AddWithValue(item.Name, item.GetValue(parameter));
                }
            }

            var count = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(count);
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string sql, object parameter = null) where T : class, new()
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new MySqlCommand(sql, conn);
            if (parameter != null)
            {
                var paramType = parameter.GetType();
                foreach (var item in paramType.GetProperties())
                {
                    cmd.Parameters.AddWithValue(item.Name, item.GetValue(parameter));
                }
            }
            var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var dataList = new List<T>();

            var objType = typeof(T);

            var properties = objType.GetProperties().Where(p => !p.GetMethod.IsVirtual).ToList();
            while (reader.Read())
            {
                var entity = new T();
                foreach (var item in properties)
                {
                    SetValue(item, entity, reader);
                }
                dataList.Add(entity);
            }
            return dataList;
        }

        private static void SetValue<T>(PropertyInfo property, T entity, MySqlDataReader reader)
        {
            switch (property.PropertyType.Name)
            {
                case "Guid":
                property.SetValue(entity, new Guid((string)reader[property.Name]));
                return;
                case "Nullable`1":
                    {
                        var rowValue = reader[property.Name];
                        if (rowValue is DBNull)
                        {
                            property.SetValue(entity, null);
                            return;
                        }

                        var paramType = property.PropertyType.GetGenericArguments()[0];
                        switch (paramType.Name)
                        {
                            case "Guid":
                            property.SetValue(entity, new Guid?(new Guid((string)rowValue)));
                            break;
                            case "DateTime":
                            property.SetValue(entity, new DateTime?((DateTime)rowValue));
                            break;
                        }
                        return;
                    }
                default:
                property.SetValue(entity, reader[property.Name]);
                break;
            }
        }
    }
}
