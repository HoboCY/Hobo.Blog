using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId);

            builder.HasMany(p => p.PostCategories)
                .WithOne(p => p.Post)
                .HasForeignKey(pc => pc.PostId);
        }
    }
}
