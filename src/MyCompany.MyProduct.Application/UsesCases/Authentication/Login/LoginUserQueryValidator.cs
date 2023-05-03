using FluentValidation;
using MyCompany.MyProduct.Application.Errors;
using MyCompany.MyProduct.Application.Extensions;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Login;

internal sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(ValidationErrors.Authentication.EmailIsRequired);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(ValidationErrors.Authentication.PasswordIsRequired);
    }
}