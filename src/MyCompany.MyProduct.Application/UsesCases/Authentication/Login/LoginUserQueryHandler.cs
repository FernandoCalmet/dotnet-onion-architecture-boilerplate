using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
using MyCompany.MyProduct.Application.Errors;
using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Login;

internal sealed class LoginUserQueryHandler
    : IQueryHandler<LoginUserQuery, Result<AuthenticationResult>>
{
    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserQueryHandler(IUserService userService, IJwtProvider jwtProvider)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthenticationResult>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        Maybe<UserDto> maybeUser = await _userService.FindByEmailAsync(request.Email);
        if (maybeUser.HasNoValue)
        {
            return Result.Failure<AuthenticationResult>(ValidationErrors.Authentication.InvalidEmailOrPassword);
        }

        UserDto user = maybeUser.Value;

        var emailValidationResult = await ValidateEmailConfirmationAsync(user);
        if (emailValidationResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(emailValidationResult.Error);
        }

        var credentialsValidationResult = await ValidateUserCredentialsAsync(user, request.Password);
        return credentialsValidationResult.IsFailure
            ? Result.Failure<AuthenticationResult>(credentialsValidationResult.Error)
            : Result.Success(CreateAuthenticationResult(user));
    }

    private async Task<Result> ValidateEmailConfirmationAsync(UserDto user)
    {
        var isEmailConfirmed = await _userService.IsEmailConfirmedAsync(user);
        return !isEmailConfirmed
            ? Result.Failure(ValidationErrors.Authentication.EmailNotConfirmed)
            : Result.Success();
    }

    private async Task<Result> ValidateUserCredentialsAsync(UserDto user, string password)
    {
        var isValidPassword = await _userService.CheckPasswordAsync(user, password);
        return !isValidPassword
            ? Result.Failure(ValidationErrors.Authentication.InvalidEmailOrPassword)
            : Result.Success();
    }

    private AuthenticationResult CreateAuthenticationResult(UserDto user)
    {
        var token = _jwtProvider.Generate(user.Id, user.Email);
        return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
    }
}