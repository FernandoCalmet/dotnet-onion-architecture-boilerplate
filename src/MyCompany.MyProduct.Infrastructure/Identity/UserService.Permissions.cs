using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result<IEnumerable<string>>> GetPermissions(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure<IEnumerable<string>>(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var userClaims = await _userManager.GetClaimsAsync(user);
        var permissions = userClaims.Select(c => c.Value);

        return Result.Success(permissions);
    }

    public async Task<Result> HasPermission(Guid userId, string permission)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var userClaims = await _userManager.GetClaimsAsync(user);
        var hasPermission = userClaims.Any(c => c.Value == permission);

        return hasPermission
            ? Result.Success()
            : Result.Failure(Account.UserDoesNotHavePermission);
    }
}