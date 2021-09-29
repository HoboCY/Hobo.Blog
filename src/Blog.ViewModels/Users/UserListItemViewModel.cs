using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.ViewModels.Users
{
   public class UserListItemViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email{ get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime CreationTime{ get; set; }

        public DateTime LastModifyTime { get; set; }
    }
}
