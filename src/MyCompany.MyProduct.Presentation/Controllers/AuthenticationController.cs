using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.UsesCases.Authentication.Login;
using MyCompany.MyProduct.Application.UsesCases.Authentication.Register;
using MyCompany.MyProduct.Core.Errors;
using MyCompany.MyProduct.Core.Shared;
using MyCompany.MyProduct.Presentation.Abstractions;
using MyCompany.MyProduct.Presentation.Contracts.Authentication;

namespace MyCompany.MyProduct.Presentation.Controllers;

[AllowAnonymous]

public sealed class AuthenticationController : ApiController
{
    public AuthenticationController(ISender sender) : base(sender)
    {
    }

    [HttpPost(ApiRoutes.Authentication.Login)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginUserRequest loginRequest) =>
        await Result.Create(loginRequest, DomainErrors.General.UnProcessableRequest)
            .Map(request => new LoginUserQuery(request.Email, request.Password))
            .Bind(command => Sender.Send(command))
            .Match(Ok, BadRequest);

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Authentication.Register)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(RegisterUserRequest registerRequest) =>
        await Result.Create(registerRequest, DomainErrors.General.UnProcessableRequest)
            .Map(request => new RegisterUserCommand(request.Email, request.Password, request.ConfirmPassword))
            .Bind(command => Sender.Send(command))
            .Match(Ok, BadRequest);
}