using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Register;

public sealed record RegisterUserCommand(string Email, string Password, string ConfirmPassword)
    : ICommand<Result<AuthenticationResult>>;