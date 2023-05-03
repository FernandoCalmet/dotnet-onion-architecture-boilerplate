using MyCompany.MyProduct.Core.Shared.ErrorComponent;

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

        public static Error InvalidEmailOrPassword => new(
        "Authentication.InvalidEmailOrPassword",
        "The specified email or password are incorrect.");

        internal static Error EmailNotConfirmed => new(
        "Authentication.EmailNotConfirmed",
        "The email is not confirmed.");
    }
}