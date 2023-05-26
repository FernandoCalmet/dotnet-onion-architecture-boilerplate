using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
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
        var user = await _userService.FindUserByEmail(request.Email);
        if (user.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(user.Error);
        }

        var emailValidationResult = await _userService.IsEmailConfirmed(user.Value.Id);
        if (emailValidationResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(emailValidationResult.Error);
        }

        var credentialsValidationResult = await _userService.CheckPassword(user.Value.Id, request.Password);
        return credentialsValidationResult.IsFailure
            ? Result.Failure<AuthenticationResult>(credentialsValidationResult.Error)
            : Result.Success(await CreateAuthenticationResult(user.Value));
    }

    private async Task<AuthenticationResult> CreateAuthenticationResult(UserDto user)
    {
        var token = await _jwtProvider.Generate(user);
        return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
    }
}