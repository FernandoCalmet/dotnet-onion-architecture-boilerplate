using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Register;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, Result<AuthenticationResult>>
{
    private readonly IUserService _userService;

    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<AuthenticationResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailCheckResult = await _userService.IsEmailUnique(request.Email);
        if (!emailCheckResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(emailCheckResult.Error);
        }

        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = GenerateUserNameFromEmail(request.Email)
        };
        var userCreatedResult = await _userService.CreateUser(user, request.Password);

        return userCreatedResult.IsFailure
            ? Result.Failure<AuthenticationResult>(userCreatedResult.Errors.First())
            : Result.Success(await _userService.GenerateToken(user));
    }

    private static string GenerateUserNameFromEmail(string email) => email.Split('@')[0];
}