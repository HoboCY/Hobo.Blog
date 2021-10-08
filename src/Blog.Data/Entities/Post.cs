using System;
using System.Collections.Generic;

namespace Blog.Data.Entities
{
    public class Post : ICreator<string>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ContentAbstract { get; set; }

        public List<int> CategoryIds { get; set; }

        public string CreatorId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifyTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}