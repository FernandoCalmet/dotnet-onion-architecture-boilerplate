using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var userClaims = await _userManager.GetClaimsAsync(appUser);
        return userClaims.Select(c => c.Value).ToList();
    }

    public async Task<bool> HasPermissionAsync(UserDto user, string permission)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var userClaims = await _userManager.GetClaimsAsync(appUser);
        return userClaims.Any(c => c.Value == permission);
    }
}