using Blog.Data.EntityConfigurations;
using Blog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
            builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new PostEntityTypeConfiguration());
            builder.ApplyConfiguration(new PostCategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommentReplyEntityTypeConfiguration());
        }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<CommentReply> CommentReplies { get; set; }
    }
}
