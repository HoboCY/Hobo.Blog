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
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId);

            builder.HasMany(c => c.CommentReply)
                .WithOne(cr => cr.Comment)
                .HasForeignKey(cr => cr.CommentId);
        }
    }
}
