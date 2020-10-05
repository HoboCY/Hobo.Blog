using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class PostCategoryEntityTypeConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.HasKey(pc => new { pc.PostId, pc.CategoryId });

            builder.HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId);

            builder.HasOne(pc => pc.Creator)
                .WithMany()
                .HasForeignKey(pc => pc.CreatorId);
        }
    }
}
