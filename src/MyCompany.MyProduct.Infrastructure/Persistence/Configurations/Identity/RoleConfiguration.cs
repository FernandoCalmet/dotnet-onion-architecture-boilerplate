using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Infrastructure.Persistence.Constants;

namespace MyCompany.MyProduct.Infrastructure.Persistence.Configurations.Identity;

internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable(TableNames.Roles);
    }
}