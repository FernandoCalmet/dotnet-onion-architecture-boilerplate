using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Persistence.Constants;

namespace MyCompany.MyProduct.Persistence.Identity.Configurations;

internal class UserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
    {
        builder.ToTable(TableNames.UserClaims, SchemaNames.Identity);
    }
}