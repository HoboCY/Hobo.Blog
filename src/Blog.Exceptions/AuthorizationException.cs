using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(message)
        {

        }
    }
}
