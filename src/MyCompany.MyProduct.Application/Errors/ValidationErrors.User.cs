using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.Errors;

internal static partial class ValidationErrors
{
    internal static class User
    {
        internal static Error NotFound => new(
            "User.NotFound",
            "The user with the specified identifier was not found.");

        internal static Error InvalidPermissions => new(
            "User.InvalidPermissions",
            "The current user does not have the permissions to perform that operation.");

        internal static Error DuplicateEmail => new(
            "User.DuplicateEmail",
            "The specified email is already in use.");

        internal static Error CannotChangePassword => new(
            "User.CannotChangePassword",
            "The password cannot be changed to the specified password.");
    }
}