using Mapster;
using Microsoft.AspNetCore.Identity;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> CreateUserAsync(UserDto user, string password)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id,
            UserName = user.Email,
            Email = user.Email,
        };

        var hashedPassword = _userManager.PasswordHasher.HashPassword(appUser, password);
        var result = await _userManager.CreateAsync(appUser, hashedPassword);
        return result.Succeeded
            ? Result.Success()
            : Result.Create(result.Errors.Select(x => new Error(x.Code, x.Description)));
    }

    public async Task<UserDto> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user.Adapt<UserDto>();
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is null;
    }

    public async Task<bool> CheckPasswordAsync(UserDto user, string password)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null) { return false; }

        var hashedPassword = _userManager.PasswordHasher.HashPassword(appUser, password);
        return await IsPasswordValidAsync(appUser, hashedPassword);
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