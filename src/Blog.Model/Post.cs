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
            PostCategories = new List<PostCategory>();
            Comments = new List<Comment>();
        }

        [Required]
        [Column("id", TypeName = "varchar(100)")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(220)]
        [MinLength(20)]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("content", TypeName = "longtext")]
        public string Content { get; set; }

        [Required]
        [Column("creator_id", TypeName = "varchar(100)")]
        public Guid CreatorId { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("last_modification_time")]
        public DateTime? LastModificationTime { get; set; }

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public virtual List<PostCategory> PostCategories { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}
