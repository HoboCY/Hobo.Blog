using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MySqlConnector;

namespace Blog.Data.Extensions
{
   public static class DbHelperExtensions
    {
        public static void SetParameters(this MySqlCommand cmd, object parameter)
        {
            if (parameter == null) return;

            var paramType = parameter.GetType();
            foreach (var item in paramType.GetProperties())
            {
                var itemValue = item.GetValue(parameter);
                if (itemValue == null) continue;
                cmd.Parameters.AddWithValue(item.Name, item.GetValue(parameter));
            }
        }

        public static void SetValue<TEntity>(this MySqlDataReader reader, PropertyInfo property, TEntity entity)
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
