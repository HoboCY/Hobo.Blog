using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationRoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("application_role");

            builder.Property(r => r.Id)
                .HasColumnType("varchar(50)");

            builder.Property(r => r.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(r => r.CreationTime)
                .IsRequired();

            builder.Property(r => r.LastModifierId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(r => r.LastModificationTime)
                .IsRequired(false);

            builder.Property(r => r.IsDeleted)
                .IsRequired();

            builder.Property(r => r.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(r => r.DeletionTime)
                .IsRequired(false);

            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.HasMany(r => r.RoleClaims)
                .WithOne(rc => rc.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            builder.HasOne(r => r.Creator)
                .WithMany()
                .HasForeignKey(r => r.CreatorId)
                .IsRequired();
        }
    }
}