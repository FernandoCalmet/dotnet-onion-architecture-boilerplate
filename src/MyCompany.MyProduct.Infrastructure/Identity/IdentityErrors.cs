using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal static class IdentityErrors
{
    internal static class Account
    {
        internal static Error LockedOut => new(
            "Account.LockedOut",
            "The account is locked out.");

        internal static Error UserNotFound => new(
            "Account.UserNotFound",
            "The user with the specified identifier was not found.");

        public static Error UserDoesNotHavePermission => new(
            "Account.UserDoesNotHavePermission",
            "The user does not have permission.");

        internal static Error EmailAlreadyExists => new(
            "Account.EmailAlreadyExists",
            "A user with this email address already exists.");

        internal static Error InvalidEmail => new(
            "Account.InvalidEmail",
            "The email address is invalid.");

        internal static Error EmailNotConfirmed => new(
            "Account.EmailNotConfirmed",
            "The email is not confirmed.");

        internal static Error InvalidPassword => new(
            "Account.InvalidPassword",
            "The password is invalid.");

        internal static Error UserNameAlreadyExists => new(
            "Account.UserNameAlreadyExists",
            "A user with this user name already exists.");

        internal static Error InvalidUserName => new(
            "Account.InvalidUserName",
            "The user name is invalid.");

        internal static Error PhoneNumberNotConfirmed => new(
            "Account.PhoneNumberNotConfirmed",
            "The phone number is not confirmed.");

        internal static Error InvalidPhoneNumber => new(
            "Account.InvalidPhoneNumber",
            "The phone number is invalid.");

        internal static Error PhoneNumberAlreadyExists => new(
            "Account.PhoneNumberAlreadyExists",
            "A user with this phone number already exists.");

        internal static Error PhoneNumberNotSet => new(
            "Account.PhoneNumberNotSet",
            "The phone number is not set.");

        internal static Error TwoFactorAuthenticationIsRequired => new(
            "Account.TwoFactorAuthenticationIsRequired",
            "Two factor authentication is required.");

        internal static Error InvalidToken => new(
            "Account.InvalidToken",
            "The token is invalid.");

        internal static Error AuthenticatorKeyNotFound => new(
            "Account.AuthenticatorKeyNotFound",
            "The authenticator key was not found.");
    }

    internal static class Role
    {
        internal static Error NotFound => new(
            "Role.UserNotFound",
            "The role with the specified identifier was not found.");

        internal static Error NameAlreadyExists => new(
            "Role.NameAlreadyExists",
            "A role with this name already exists.");

        internal static Error InvalidName => new(
            "Role.InvalidName",
            "The role name is invalid.");

        internal static Error RoleInUse => new(
            "Role.RoleInUse",
            "The role is in use.");

        internal static Error UserAlreadyInRole => new(
            "Role.UserAlreadyInRole",
            "The user is already in the role.");

        internal static Error UserNotInRole => new(
            "Role.UserNotInRole",
            "The user is not in the role.");

        internal static Error RoleClaimsMustBeUnique => new(
            "Role.RoleClaimsMustBeUnique",
            "Role claims must be unique.");

        internal static Error RoleClaimNotFound => new(
            "Role.RoleClaimNotFound",
            "The role claim was not found.");
    }
}