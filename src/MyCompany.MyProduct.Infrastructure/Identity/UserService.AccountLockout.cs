using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task LockAccountAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        await _userManager.SetLockoutEndDateAsync(appUser, DateTimeOffset.UtcNow.AddYears(100));
    }

    public async Task UnlockAccountAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        await _userManager.SetLockoutEndDateAsync(appUser, null);
    }

    public async Task<bool> IsLockedOutAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.IsLockedOutAsync(appUser);
    }
}