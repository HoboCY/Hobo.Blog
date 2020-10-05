using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("role")]
    public class Role
    {
        [Required]
        [Column("id", TypeName = "varchar(100)")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("normalized_name")]
        public string NormalizedName { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Column("creator_id", TypeName = "varchar(100)")]
        public Guid CreatorId { get; set; }

        [Column("last_modification_time")]
        public DateTime? LastModificationTime { get; set; }

        [Column("last_modifier_id", TypeName = "varchar(100)")]
        public Guid? LastModifierId { get; set; }

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("deleter_id", TypeName = "varchar(100)")]
        public Guid? DeleterId { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public virtual AppUser Creator { get; set; }
    }
}
