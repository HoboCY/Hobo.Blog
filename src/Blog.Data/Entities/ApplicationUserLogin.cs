using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Data.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}