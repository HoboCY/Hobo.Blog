using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("user_role")]
    public class UserRole
    {
        [Required]
        [Column("user_id", TypeName = "varchar(100)")]
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

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

        public virtual AppUser AppUser { get; set; }

        public virtual AppUser Creator { get; set; }

        public virtual Role Role { get; set; }
    }
}
