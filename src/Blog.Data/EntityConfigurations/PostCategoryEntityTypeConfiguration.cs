using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

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

            builder.HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired();
        }
    }
}