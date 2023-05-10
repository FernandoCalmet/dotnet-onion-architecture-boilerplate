using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Persistence.Constants;

namespace MyCompany.MyProduct.Persistence.Identity.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(TableNames.Users, SchemaNames.Identity);
    }
}