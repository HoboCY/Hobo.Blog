using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("post")]
    public class Post
    {
        public Post()
        {
            PostCategories = new HashSet<PostCategory>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Content { get; set; }

        public string ContentAbstract { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModificationTime { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }

        public virtual ApplicationUser Creator { get; set; }
    }
}