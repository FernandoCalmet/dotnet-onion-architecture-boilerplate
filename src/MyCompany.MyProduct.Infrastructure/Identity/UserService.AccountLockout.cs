using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> LockAccount(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> UnlockAccount(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.SetLockoutEndDateAsync(user, null);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> IsLockedOut(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var isLockedOut = await _userManager.IsLockedOutAsync(user);

        return !isLockedOut
            ? Result.Success(isLockedOut)
            : Result.Failure<bool>(IdentityErrors.Account.LockedOut);
    }
}