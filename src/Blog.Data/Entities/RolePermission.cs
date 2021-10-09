using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Entities
{
    public class RolePermission
    {
        public long Id { get; set; }

        public int RoleId { get; set; }

        public List<string> Permissions { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
