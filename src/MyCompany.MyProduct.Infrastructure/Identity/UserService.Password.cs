using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> GeneratePasswordResetToken(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.GeneratePasswordResetTokenAsync(user.Value);

        return Result.Success(result);
    }

    public async Task<Result> ResetPassword(Guid userId, string token, string newPassword)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.ResetPasswordAsync(user.Value, token, newPassword);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> ChangePassword(Guid userId, string currentPassword, string newPassword)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.ChangePasswordAsync(user.Value, currentPassword, newPassword);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }
}