using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
using MyCompany.MyProduct.Application.Errors;
using MyCompany.MyProduct.Core.Shared.ResultComponent;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Register;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, Result<AuthenticationResult>>
{
    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUserCommandHandler(IUserService userService, IJwtProvider jwtProvider)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthenticationResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailCheckResult = await EnsureEmailIsUniqueAsync(request.Email);
        if (emailCheckResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(emailCheckResult.Error);
        }

        var user = new UserDto(Guid.NewGuid(), request.Email);
        var userCreatedResult = await _userService.CreateUserAsync(user, request.Password);
        return userCreatedResult.IsFailure
            ? Result.Failure<AuthenticationResult>(userCreatedResult.Error)
            : Result.Success(CreateAuthenticationResult(user));
    }

    private async Task<Result> EnsureEmailIsUniqueAsync(string email)
    {
        if (await _userService.IsEmailUniqueAsync(email))
        {
            return Result.Success();
        }

        return Result.Failure(ValidationErrors.User.DuplicateEmail);
    }

    private AuthenticationResult CreateAuthenticationResult(UserDto user)
    {
        var token = _jwtProvider.Generate(user.Id, user.Email);
        return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
    }
}