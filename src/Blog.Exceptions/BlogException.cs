using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Exceptions
{
    public class BlogException : Exception
    {
        public BlogException(string message) : base(message)
        {

        }

        public BlogException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public BlogException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public BlogException(int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
