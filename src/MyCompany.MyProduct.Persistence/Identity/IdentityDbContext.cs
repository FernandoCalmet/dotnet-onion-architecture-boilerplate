using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Persistence.Identity.Configurations;

namespace MyCompany.MyProduct.Persistence.Identity;

public class IdentityDbContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    Guid,
    ApplicationUserClaim,
    ApplicationUserRole,
    ApplicationUserLogin,
    ApplicationRoleClaim,
    ApplicationUserToken>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
        modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
    }
}