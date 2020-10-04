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
        [Required]
        [Column("category_id", TypeName = "varchar(100)")]
        public Guid CategoryId { get; set; }

        [Required]
        [Column("post_id", TypeName = "varchar(100)")]
        public Guid PostId { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Column("creator_id", TypeName = "varchar(100)")]
        public Guid CreatorId { get; set; }

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("deleter_id", TypeName = "varchar(100)")]
        public Guid? DeleterId { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public virtual Post Post { get; set; }

        public virtual Category Category { get; set; }
    }
}
