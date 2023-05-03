using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task EnableTwoFactorAuthenticationAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        await _userManager.SetTwoFactorEnabledAsync(appUser, true);
    }

    public async Task DisableTwoFactorAuthenticationAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        await _userManager.SetTwoFactorEnabledAsync(appUser, false);
    }

    public async Task<bool> IsTwoFactorAuthenticationEnabledAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.GetTwoFactorEnabledAsync(appUser);
    }

    public async Task<bool> HasAuthenticatorAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(appUser);
        return !string.IsNullOrEmpty(authenticatorKey);
    }
}