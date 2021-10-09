using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Entities
{
    public class UserRole
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public int RoleId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
