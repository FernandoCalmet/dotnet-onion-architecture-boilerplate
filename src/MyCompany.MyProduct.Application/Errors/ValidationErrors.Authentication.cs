using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.Errors;

internal static partial class ValidationErrors
{
    internal static class Authentication
    {
        internal static Error EmailIsRequired => new(
            "Authentication.EmailIsRequired",
            "The email is required.");

        internal static Error PasswordIsRequired => new(
            "Authentication.PasswordIsRequired",
            "The password is required.");

        internal static Error InvalidEmail => new(
            "Authentication.InvalidEmail",
            "The specified email is incorrect.");

        internal static Error InvalidEmailOrPassword => new(
            "Authentication.InvalidEmailOrPassword",
            "The specified email or password are incorrect.");

        internal static Error EmailNotConfirmed => new(
            "Authentication.EmailNotConfirmed",
            "The email is not confirmed.");

        internal static Error PasswordDoNotMatch => new(
            "Authentication.PasswordDoNotMatch",
            "Passwords do not match.");

        internal static Error InvalidPasswordLength(int passwordLength) => new(
            "Authentication.InvalidPasswordLength",
            $"The password must have at least {passwordLength} characters.");

        internal static Error InvalidPasswordLeastOneCapitalLetter => new(
            "Authentication.InvalidPasswordLeastOneCapitalLetter",
            "The password must contain at least one capital letter.");

        internal static Error InvalidPasswordLeastOneLowercaseLetter => new(
            "Authentication.InvalidPasswordLeastOneLowercaseLetter",
            "The password must contain at least one lowercase letter.");

        internal static Error InvalidPasswordLeastOneNumber => new(
            "Authentication.InvalidPasswordLeastOneNumber",
            "The password must contain at least one number.");

        internal static Error InvalidPasswordLeastOneSpecialCharacter => new(
            "Authentication.InvalidPasswordLeastOneSpecialCharacter",
            "The password must contain at least one special character.");
    }
}