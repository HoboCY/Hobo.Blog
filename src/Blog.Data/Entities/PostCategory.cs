using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Entities
{
    [Table("post_category")]
    public class PostCategory
    {
        public Guid CategoryId { get; set; }

        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual Category Category { get; set; }
    }
}