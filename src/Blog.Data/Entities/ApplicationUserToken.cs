using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Data.Entities
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}