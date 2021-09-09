using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class PostManageViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
