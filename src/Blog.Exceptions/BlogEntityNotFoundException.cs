using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Exceptions
{
    public class BlogEntityNotFoundException : Exception
    {
        public Type EntityType { get; set; }

        public string Parameter { get; set; }

        public BlogEntityNotFoundException(string message) : base(message)
        {

        }

        public BlogEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public BlogEntityNotFoundException(Type entityType, string parameter, Exception innerException)
            : base(
                string.IsNullOrWhiteSpace(parameter)
                    ? $"There is no such an entity given parameter. Entity type：{entityType.Name}"
                    : $"There is no such an entity. Entity type：{entityType.Name}，parameter：{parameter}", innerException)
        {
            EntityType = entityType;
            Parameter = parameter;
        }

        public BlogEntityNotFoundException(Type entityType, string parameter)
            : this(entityType, parameter, null)
        {
        }
    }
}
