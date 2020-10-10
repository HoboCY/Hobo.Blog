using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string Mobile { get; set; }

        public DateTime LastLoginTime { get; set; }
    }
}
