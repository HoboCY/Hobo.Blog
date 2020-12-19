using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

namespace Blog.Data.EntityConfigurations
{
    public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("post");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Content)
                .HasColumnType("longtext")
                .IsRequired();

            builder.Property(p => p.ContentAbstract)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(p => p.CreatorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.CreationTime)
                .IsRequired();

            builder.Property(r => r.LastModificationTime)
                .IsRequired(false);

            builder.Property(p => p.IsDeleted)
                .IsRequired();

            builder.Property(p => p.DeleterId)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(p => p.DeletionTime)
                .IsRequired(false);

            builder.HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .IsRequired();

            builder.HasMany(p => p.PostCategories)
                .WithOne(pc => pc.Post)
                .HasForeignKey(pc => pc.PostId)
                .IsRequired();

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}