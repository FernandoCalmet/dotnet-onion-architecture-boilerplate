using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Persistence.Constants;

namespace MyCompany.MyProduct.Persistence.Identity.Configurations;

internal class UserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable(TableNames.UserRoles, SchemaNames.Identity);

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<ApplicationRole>()
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}