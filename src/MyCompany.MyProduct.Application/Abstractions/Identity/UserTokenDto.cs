namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class UserTokenDto
{
    public Guid UserId { get; set; }
    public string LoginProvider { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}