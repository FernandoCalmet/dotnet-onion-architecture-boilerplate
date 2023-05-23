using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> EnableTwoFactorAuthentication(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.SetTwoFactorEnabledAsync(user, true);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> DisableTwoFactorAuthentication(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> IsTwoFactorAuthenticationEnabled(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.GetTwoFactorEnabledAsync(user);
        return Result.Success(identityResult);
    }

    public async Task<Result> HasAuthenticator(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        return !string.IsNullOrEmpty(authenticatorKey)
            ? Result.Success(authenticatorKey)
            : Result.Failure(Account.AuthenticatorKeyNotFound);
    }
}