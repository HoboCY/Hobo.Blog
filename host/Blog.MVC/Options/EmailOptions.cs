using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Options
{
    public class EmailOptions
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public  string Password { get; set; }

        public  string Host { get; set; }

        public  int Port { get; set; }
    }
}
