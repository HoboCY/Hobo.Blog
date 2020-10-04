using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("comment")]
    public class Comment
    {
        public Comment()
        {
            CommentReply = new List<CommentReply>();
        }

        [Required]
        [Column("id", TypeName = "varchar(100)")]
        public Guid Id { get; set; }

        [Required]
        [Column("post_id", TypeName = "varchar(100)")]
        public Guid PostId { get; set; }

        [Required]
        [Column("comment_content")]
        [MaxLength(250)]
        public string CommentContent { get; set; }

        [Required]
        [Column("creator_id", TypeName = "varchar(100)")]
        public Guid CreatorId { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual Post Post { get; set; }

        public virtual List<CommentReply> CommentReply { get; set; }
    }
}
