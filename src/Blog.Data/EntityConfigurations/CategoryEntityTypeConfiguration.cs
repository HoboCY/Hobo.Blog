using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.EntityConfigurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnType("varchar(50)");

            builder.Property(c => c.CategoryName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.NormalizedCategoryName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(c => c.CreationTime)
                .IsRequired();

            builder.Property(c => c.LastModifierId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(c => c.LastModificationTime)
                .IsRequired(false);

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
                .IsRequired(false);

            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.HasData(
                new Category 
                { 
                    Id = Guid.NewGuid(), 
                    CategoryName = "asp .net core", 
                    NormalizedCategoryName = "asp .net core".ToUpper(), 
                    CreationTime = DateTime.UtcNow 
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "c#",
                    NormalizedCategoryName = "c#".ToUpper(),
                    CreationTime = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "asp .net core mvc",
                    NormalizedCategoryName = "asp .net core mvc".ToUpper(),
                    CreationTime = DateTime.UtcNow
                });
        }
    }
}