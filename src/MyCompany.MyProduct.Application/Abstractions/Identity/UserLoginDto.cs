namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class UserLoginDto
{
    public Guid UserId { get; set; }
    public string LoginProvider { get; set; } = default!;
    public string ProviderKey { get; set; } = default!;
    public string ProviderDisplayName { get; set; } = default!;
}