using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> GeneratePasswordResetTokenAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.GeneratePasswordResetTokenAsync(appUser);
    }

    public async Task<string> ResetPasswordAsync(UserDto user, string token, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        var result = await _userManager.ResetPasswordAsync(appUser, token, newPassword);
        return result.Succeeded ? null : string.Join(", ", result.Errors.Select(e => e.Description));
    }
}