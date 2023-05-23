using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> GenerateEmailConfirmationToken(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var result = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return Result.Success(result);
    }

    public async Task<Result> ConfirmEmail(Guid userId, string token)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded
            ? Result.Success()
            : Result.Create(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> IsEmailConfirmed(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        return isEmailConfirmed
            ? Result.Success(isEmailConfirmed)
            : Result.Failure(IdentityErrors.Account.EmailNotConfirmed);
    }

    public async Task<Result> ConfirmPhoneNumber(Guid userId, string code)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;

        if (user.PhoneNumber is null)
        {
            return Result.Failure(IdentityErrors.Account.PhoneNumberNotSet);
        }

        var identityResult = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);
        var result = Result.Create(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> IsPhoneNumberConfirmed(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(IdentityErrors.Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var isPhoneNumberConfirmed = await _userManager.IsPhoneNumberConfirmedAsync(user);

        return isPhoneNumberConfirmed
            ? Result.Success(isPhoneNumberConfirmed)
            : Result.Failure(IdentityErrors.Account.PhoneNumberNotConfirmed);
    }
}