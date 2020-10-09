using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");

            builder.Property(au => au.Id)
                .HasColumnType("varchar(50)");

            builder.Property(au => au.NickName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(au => au.UserName)
                .HasMaxLength(50);

            builder.Property(au => au.NormalizedUserName)
                .HasMaxLength(50);

            builder.Property(au => au.Email)
                .HasMaxLength(50);

            builder.Property(au => au.NormalizedEmail)
                .HasMaxLength(50);

            builder.Property(au => au.PasswordHash)
                .HasMaxLength(100);

            builder.Property(au => au.CreationTime).IsRequired();
            builder.Property(au => au.LastLoginTime).IsRequired(false);
            builder.Property(au => au.LastModificationTime).IsRequired(false);

            // Each User can have many UserClaims
            builder.HasMany(au => au.Claims)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            builder.HasMany(au => au.Logins)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            builder.HasMany(au => au.Tokens)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany(au => au.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}