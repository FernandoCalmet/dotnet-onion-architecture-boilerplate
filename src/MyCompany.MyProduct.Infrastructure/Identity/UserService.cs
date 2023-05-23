using Mapster;
using Microsoft.AspNetCore.Identity;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

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

    public async Task<Result> CreateUser(UserDto user, string password)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id,
            UserName = user.Email,
            Email = user.Email,
        };

        var hashedPassword = _userManager.PasswordHasher.HashPassword(appUser, password);
        var identityResult = await _userManager.CreateAsync(appUser, hashedPassword);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> UpdateUser(UserDto user)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(user.Id.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var appUser = maybeUser.Value;
        appUser.Email = user.Email;
        appUser.PhoneNumber = user.PhoneNumber;

        var identityResult = await _userManager.UpdateAsync(appUser);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> DeleteUser(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.DeleteAsync(user);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result<UserDto>> FindById(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure<UserDto>(Account.UserNotFound);
        }

        var user = maybeUser.Value.Adapt<UserDto>();
        return Result.Success(user);
    }

    public async Task<Result<UserDto>> FindByEmail(string email)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByEmailAsync(email) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure<UserDto>(Account.UserNotFound);
        }

        var user = maybeUser.Value.Adapt<UserDto>();
        return Result.Success(user);
    }

    public async Task<Result> IsEmailUnique(string email)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByEmailAsync(email) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        return maybeUser.Value.Email == email
            ? Result.Failure<bool>(Account.EmailAlreadyExists)
            : Result.Success();
    }

    public async Task<Result> CheckPassword(Guid userId, string password)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var hashedPassword = _userManager.PasswordHasher.HashPassword(user, password);
        var isPasswordValid = await IsPasswordValid(user, hashedPassword);

        return isPasswordValid
            ? Result.Success(isPasswordValid)
            : Result.Failure<bool>(Account.InvalidPassword);
    }

    public async Task<Result> HasPassword(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var hasPassword = await _userManager.HasPasswordAsync(user);

        return hasPassword
            ? Result.Success(hasPassword)
            : Result.Failure<bool>(Account.InvalidPassword);
    }

    private async Task<bool> IsPasswordValid(ApplicationUser user, string password)
    {
        var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(user, password);
    }
}