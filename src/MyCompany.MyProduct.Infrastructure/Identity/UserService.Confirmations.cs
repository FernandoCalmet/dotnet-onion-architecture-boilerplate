using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> GenerateEmailConfirmationToken(Guid userId)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.GenerateEmailConfirmationTokenAsync(user.Value);

        return Result.Success(result);
    }

    public async Task<Result> ConfirmEmail(Guid userId, string token)
    {
        var user = await GetUserById(userId);

        var result = await _userManager.ConfirmEmailAsync(user.Value, token);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> IsEmailConfirmed(Guid userId)
    {
        var user = await GetUserById(userId);

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user.Value);

        return isEmailConfirmed
            ? Result.Success(isEmailConfirmed)
            : Result.Failure(IdentityErrors.Account.EmailNotConfirmed);
    }

    public async Task<Result> ConfirmPhoneNumber(Guid userId, string code)
    {
        var user = await GetUserById(userId);

        if (user.Value.PhoneNumber is null)
        {
            return Result.Failure(IdentityErrors.Account.PhoneNumberNotSet);
        }

        var result = await _userManager.ChangePhoneNumberAsync(user.Value, user.Value.PhoneNumber, code);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> IsPhoneNumberConfirmed(Guid userId)
    {
        var user = await GetUserById(userId);

        var isPhoneNumberConfirmed = await _userManager.IsPhoneNumberConfirmedAsync(user.Value);

        return isPhoneNumberConfirmed
            ? Result.Success(isPhoneNumberConfirmed)
            : Result.Failure(IdentityErrors.Account.PhoneNumberNotConfirmed);
    }
}