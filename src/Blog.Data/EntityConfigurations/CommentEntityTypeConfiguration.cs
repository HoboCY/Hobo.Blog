using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnType("varchar(50)");

            builder.Property(c => c.PostId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.CommentContent)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.CreationTime)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .IsRequired();

            builder.Property(c => c.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(c => c.DeletionTime)
                .IsRequired(false);

            builder.HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId)
                .IsRequired();

            builder.HasMany(c => c.CommentReplies)
                .WithOne(cr => cr.Comment)
                .HasForeignKey(cr => cr.CommentId)
                .IsRequired();
        }
    }
}