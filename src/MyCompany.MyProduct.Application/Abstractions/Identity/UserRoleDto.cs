namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class UserRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}