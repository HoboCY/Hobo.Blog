using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ContentAbstract { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid CreatorId { get; set; }

        public string UserName { get; set; }
    }
}
