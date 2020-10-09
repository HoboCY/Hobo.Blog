using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("post_category")]
    public class PostCategory
    {
        public Guid CategoryId { get; set; }

        public Guid PostId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual Post Post { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser Creator { get; set; }
    }
}