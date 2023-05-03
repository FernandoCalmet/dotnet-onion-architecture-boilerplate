using Mapster;
using Microsoft.AspNetCore.Identity;
using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UserDto> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user.Adapt<UserDto>();
    }

    public async Task<bool> CheckPasswordAsync(UserDto user, string password)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null) { return false; }

        return await IsPasswordValidAsync(appUser, password);
    }

    public async Task<bool> HasPasswordAsync(UserDto user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null) { return false; }
        return await _userManager.HasPasswordAsync(appUser);
    }

    private async Task<bool> IsPasswordValidAsync(ApplicationUser appUser, string password)
    {
        var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(appUser, password);
    }
}