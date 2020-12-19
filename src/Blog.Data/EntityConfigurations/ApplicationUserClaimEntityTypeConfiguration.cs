using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable("application_user_claim");

            builder.Property(uc => uc.UserId)
                .HasColumnType("varchar(50)");
        }
    }
}