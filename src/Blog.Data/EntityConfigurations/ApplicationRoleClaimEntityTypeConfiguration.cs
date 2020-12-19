using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("application_role_claim");

            builder.Property(rc => rc.RoleId)
                .HasColumnType("varchar(50)");
        }
    }
}