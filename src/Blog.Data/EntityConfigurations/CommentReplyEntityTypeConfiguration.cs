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
            builder.HasKey(cr => cr.Id);

            builder.HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId);
        }
    }
}
