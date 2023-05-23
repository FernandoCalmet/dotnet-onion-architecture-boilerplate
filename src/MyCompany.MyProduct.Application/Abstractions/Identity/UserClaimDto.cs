namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class UserClaimDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
}