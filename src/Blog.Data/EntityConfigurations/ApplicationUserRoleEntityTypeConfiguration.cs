using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("application_user_role");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Property(ur => ur.UserId)
                .HasColumnType("varchar(50)");

            builder.Property(ur => ur.RoleId)
                .HasColumnType("varchar(50)");

            builder.Property(ur => ur.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(ur => ur.CreationTime)
                .IsRequired();

            builder.Property(ur => ur.IsDeleted)
                .IsRequired();

            builder.Property(ur => ur.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(ur => ur.DeletionTime)
                .IsRequired(false);

            builder.HasOne(ur => ur.Creator)
                .WithMany()
                .HasForeignKey(ur => ur.CreatorId)
                .IsRequired();
        }
    }
}