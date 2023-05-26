using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result<IEnumerable<string>>> GetPermissions(Guid userId)
    {
        var user = await GetUserById(userId);

        var userClaims = await _userManager.GetClaimsAsync(user.Value);
        var permissions = userClaims.Select(c => c.Value);

        return Result.Success(permissions);
    }

    public async Task<Result> HasPermission(Guid userId, string permission)
    {
        var user = await GetUserById(userId);

        var userClaims = await _userManager.GetClaimsAsync(user.Value);
        var hasPermission = userClaims.Any(c => c.Value == permission);

        return hasPermission
            ? Result.Success()
            : Result.Failure(Account.UserDoesNotHavePermission);
    }
}