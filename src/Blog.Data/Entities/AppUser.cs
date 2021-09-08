using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifyTime { get; set; }
    }
}