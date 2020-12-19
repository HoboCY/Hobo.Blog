using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Data.Entities
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}