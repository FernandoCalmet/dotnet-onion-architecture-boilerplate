using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> LockAccount(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.SetLockoutEndDateAsync(user.Value, DateTimeOffset.UtcNow.AddYears(100));
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> UnlockAccount(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.SetLockoutEndDateAsync(user.Value, null);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> IsLockedOut(Guid userId)
    {
        var user = await GetUserById(userId);

        var isLockedOut = await _userManager.IsLockedOutAsync(user.Value);

        return !isLockedOut
            ? Result.Success(isLockedOut)
            : Result.Failure<bool>(IdentityErrors.Account.LockedOut);
    }
}