using Blog.Data.EntityConfigurations;
using Blog.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationUserClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationUserLoginEntityTypeConfiguration());
            builder.ApplyConfiguration(new ApplicationUserTokenEntityTypeConfiguration());
            builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new PostEntityTypeConfiguration());
            builder.ApplyConfiguration(new PostCategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommentReplyEntityTypeConfiguration());
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<CommentReply> CommentReplies { get; set; }
    }
}