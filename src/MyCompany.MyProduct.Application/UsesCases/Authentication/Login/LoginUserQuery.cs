using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Messaging;
using MyCompany.MyProduct.Core.Shared.ResultComponent;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Login;

public sealed record LoginUserQuery(string Email, string Password)
    : IQuery<Result<AuthenticationResult>>;