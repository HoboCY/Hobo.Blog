using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Entities
{
    public class Post
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public string ContentAbstract { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifyTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}