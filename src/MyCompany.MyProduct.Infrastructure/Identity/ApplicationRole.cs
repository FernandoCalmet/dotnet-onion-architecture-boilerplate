using Microsoft.AspNetCore.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;
}