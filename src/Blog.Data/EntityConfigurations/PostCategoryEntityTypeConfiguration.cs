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
            builder.ToTable("post_category");

            builder.HasKey(pc => new { pc.PostId, pc.CategoryId });

            builder.Property(pc => pc.PostId)
                .HasColumnType("varchar(50)");

            builder.Property(pc => pc.CategoryId)
               .HasColumnType("varchar(50)");

            builder.Property(pc => pc.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(pc => pc.CreationTime)
                .IsRequired();

            builder.Property(pc => pc.IsDeleted)
                .IsRequired();

            builder.Property(pc => pc.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(pc => pc.DeletionTime)
                .IsRequired(false);

            builder.HasOne(pc => pc.Creator)
                .WithMany()
                .HasForeignKey(pc => pc.CreatorId)
                .IsRequired();

            builder.HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired();

            builder.HasQueryFilter(pc => !pc.IsDeleted);
        }
    }
}