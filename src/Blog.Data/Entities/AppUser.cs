using System;

namespace Blog.Data.Entities
{
    public class AppUser
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifyTime { get; set; }
    }
}