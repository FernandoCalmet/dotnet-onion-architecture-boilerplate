using Microsoft.AspNetCore.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;
    public ICollection<ApplicationUserToken> UserTokens { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}