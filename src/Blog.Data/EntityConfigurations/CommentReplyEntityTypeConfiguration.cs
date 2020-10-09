using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class CommentReplyEntityTypeConfiguration : IEntityTypeConfiguration<CommentReply>
    {
        public void Configure(EntityTypeBuilder<CommentReply> builder)
        {
            builder.ToTable("comment_reply");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Id)
                .HasColumnType("varchar(50)");

            builder.Property(cr => cr.CommentId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(cr => cr.ReplyContent)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(cr => cr.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(cr => cr.CreationTime)
                .IsRequired();

            builder.Property(cr => cr.IsDeleted)
                .IsRequired();

            builder.Property(cr => cr.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(cr => cr.DeletionTime)
                .IsRequired(false);

            builder.HasOne(cr => cr.Creator)
                .WithMany()
                .HasForeignKey(cr => cr.CreatorId)
                .IsRequired();
        }
    }
}