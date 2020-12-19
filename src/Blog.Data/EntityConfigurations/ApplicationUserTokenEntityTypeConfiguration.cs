using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.ToTable("application_user_token");

            builder.HasKey(ut => ut.UserId);

            builder.Property(ut => ut.UserId)
                .HasColumnType("varchar(50)");
        }
    }
}