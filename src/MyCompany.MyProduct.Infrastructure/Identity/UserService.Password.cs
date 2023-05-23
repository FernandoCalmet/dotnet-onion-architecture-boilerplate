using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> GeneratePasswordResetToken(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var result = await _userManager.GeneratePasswordResetTokenAsync(user);
        return Result.Success(result);
    }

    public async Task<Result> ResetPassword(Guid userId, string token, string newPassword)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> ChangePassword(Guid userId, string currentPassword, string newPassword)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;

        if (maybeUser.HasNoValue)
        {
            return Result.Failure<ApplicationUser>(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        var result = Result.Create(identityResult.Errors.Select(x => new Error(x.Code, x.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }
}