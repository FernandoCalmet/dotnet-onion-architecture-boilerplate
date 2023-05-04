using FluentValidation;
using MyCompany.MyProduct.Application.Errors;
using MyCompany.MyProduct.Application.Extensions;

namespace MyCompany.MyProduct.Application.UsesCases.Authentication.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const int PasswordLength = 8;

    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithError(ValidationErrors.Authentication.EmailIsRequired)
            .EmailAddress().WithError(ValidationErrors.Authentication.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().NotEmpty().WithError(ValidationErrors.Authentication.PasswordIsRequired)
            .MinimumLength(PasswordLength)
            .WithError(ValidationErrors.Authentication.InvalidPasswordLength(PasswordLength))
            .Matches("[A-Z]").WithError(ValidationErrors.Authentication.InvalidPasswordLeastOneCapitalLetter)
            .Matches("[a-z]").WithError(ValidationErrors.Authentication.InvalidPasswordLeastOneLowercaseLetter)
            .Matches("[0-9]").WithError(ValidationErrors.Authentication.InvalidPasswordLeastOneNumber)
            .Matches("[^a-zA-Z0-9]").WithError(ValidationErrors.Authentication.InvalidPasswordLeastOneSpecialCharacter);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithError(ValidationErrors.Authentication.PasswordDoNotMatch);
    }
}