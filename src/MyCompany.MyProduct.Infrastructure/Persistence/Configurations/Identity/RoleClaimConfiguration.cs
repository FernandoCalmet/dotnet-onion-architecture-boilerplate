using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Infrastructure.Persistence.Constants;

namespace MyCompany.MyProduct.Infrastructure.Persistence.Configurations.Identity;

internal class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        builder.ToTable(TableNames.RoleClaims);
    }
}