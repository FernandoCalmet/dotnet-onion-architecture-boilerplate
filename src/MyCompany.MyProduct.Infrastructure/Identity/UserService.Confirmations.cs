using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> GenerateEmailConfirmationTokenAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
    }

    public async Task<string> ConfirmEmailAsync(UserDto user, string token)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var result = await _userManager.ConfirmEmailAsync(appUser, token);
        return result.Succeeded ? null : string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async Task<bool> IsEmailConfirmedAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.IsEmailConfirmedAsync(appUser);
    }

    public async Task<string> ConfirmPhoneNumberAsync(UserDto user, string code)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var result = await _userManager.ChangePhoneNumberAsync(appUser, appUser.PhoneNumber, code);
        return result.Succeeded ? null : string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async Task<bool> IsPhoneNumberConfirmedAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.IsPhoneNumberConfirmedAsync(appUser);
    }

}