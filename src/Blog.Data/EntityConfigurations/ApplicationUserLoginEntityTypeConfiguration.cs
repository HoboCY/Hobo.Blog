using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Entities;

namespace Blog.Data.EntityConfigurations
{
    public class ApplicationUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable("application_user_login");

            builder.HasKey(ul => ul.UserId);

            builder.Property(ul => ul.UserId)
                .HasColumnType("varchar(50)");
        }
    }
}