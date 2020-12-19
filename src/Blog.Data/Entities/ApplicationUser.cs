using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Blog.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string NickName { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastModificationTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }

        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }

        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}