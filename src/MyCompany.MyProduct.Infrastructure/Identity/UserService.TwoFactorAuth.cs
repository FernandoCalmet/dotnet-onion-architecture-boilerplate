using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> EnableTwoFactorAuthentication(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.SetTwoFactorEnabledAsync(user.Value, true);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> DisableTwoFactorAuthentication(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.SetTwoFactorEnabledAsync(user.Value, false);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> IsTwoFactorAuthenticationEnabled(Guid userId)
    {
        var user = await GetUserById(userId);

        var identityResult = await _userManager.GetTwoFactorEnabledAsync(user.Value);

        return Result.Success(identityResult);
    }

    public async Task<Result> HasAuthenticator(Guid userId)
    {
        var user = await GetUserById(userId);

        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user.Value);

        return !string.IsNullOrEmpty(authenticatorKey)
            ? Result.Success(authenticatorKey)
            : Result.Failure(Account.AuthenticatorKeyNotFound);
    }
}