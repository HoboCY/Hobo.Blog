using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    [Table("app_user")]
    public class AppUser
    {
        [Required]
        [Column("id", TypeName = "varchar(100)")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        [Column("normalized_username")]
        public string NormalizedUsername { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        [Column("normalized_email")]
        public string NormalizedEmail { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(11)]
        [Column("mobile")]
        public string Mobile { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("last_modification_time")]
        public DateTime? LastModificationTime { get; set; }

        [Column("last_modifier_id", TypeName = "varchar(100)")]
        public Guid? LastModifierId { get; set; }

        [Column("last_login_time")]
        public DateTime? LastLoginTime { get; set; }

        [Required]
        [Column("status")]
        public bool Status { get; set; }

    }
}
