using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
