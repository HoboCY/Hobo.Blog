using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Exceptions
{
    public class BlogEntityNotFoundException : Exception
    {
        public Type EntityType { get; set; }

        public object Parameter { get; set; }

        public BlogEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public BlogEntityNotFoundException(Type entityType, object parameter, Exception innerException)
            : base(
                parameter == null
                    ? $"There is no such an entity given parameter. Entity type：{entityType.Name}"
                    : $"There is no such an entity. Entity type：{entityType.Name}，parameter：{parameter}", innerException)
        {
            EntityType = entityType;
            Parameter = parameter;
        }

        public BlogEntityNotFoundException(Type entityType, object parameter)
            : this(entityType, parameter, null)
        {
        }
    }
}
