using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Persistence.Constants;

namespace MyCompany.MyProduct.Persistence.Identity.Configurations;

internal class RoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.ToTable(TableNames.RoleClaims, SchemaNames.Identity);
    }
}