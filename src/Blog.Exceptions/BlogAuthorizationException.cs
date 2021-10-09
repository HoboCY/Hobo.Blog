using System;

namespace Blog.Exceptions
{
    public class BlogAuthorizationException : Exception
    {
        public int StatusCode { get; set; }

        public BlogAuthorizationException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
