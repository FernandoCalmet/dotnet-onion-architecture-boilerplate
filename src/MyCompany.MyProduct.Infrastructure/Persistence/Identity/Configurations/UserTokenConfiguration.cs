using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Infrastructure.Persistence.Constants;

namespace MyCompany.MyProduct.Infrastructure.Persistence.Identity.Configurations;

internal class UserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.ToTable(TableNames.UserTokens, SchemaNames.Identity);

        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.UserTokens)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}