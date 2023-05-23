namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class RoleClaimDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
}