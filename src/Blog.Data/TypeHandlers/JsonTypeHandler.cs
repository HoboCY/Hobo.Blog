using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;

namespace Blog.Data.TypeHandlers
{
    public class JsonTypeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public object Parse(Type destinationType, object value)
        {
            return JsonSerializer.Deserialize((string) value, destinationType);
        }
    }
}
