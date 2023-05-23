using MyCompany.MyProduct.Core.Shared;

namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public interface IUserService
{
    // User
    Task<Result> CreateUser(UserDto user, string password);
    Task<Result> UpdateUser(UserDto user);
    Task<Result> DeleteUser(Guid userId);
    Task<Result<UserDto>> FindById(Guid userId);
    Task<Result<UserDto>> FindByEmail(string email);
    Task<Result> IsEmailUnique(string email);
    Task<Result> CheckPassword(Guid userId, string password);
    Task<Result> HasPassword(Guid userId);

    // Two-Factor Authentication
    Task<Result> EnableTwoFactorAuthentication(Guid userId);
    Task<Result> DisableTwoFactorAuthentication(Guid userId);
    Task<Result> IsTwoFactorAuthenticationEnabled(Guid userId);
    Task<Result> HasAuthenticator(Guid userId);

    // Account Lockout
    Task<Result> LockAccount(Guid userId);
    Task<Result> UnlockAccount(Guid userId);
    Task<Result> IsLockedOut(Guid userId);

    // Email & Phone Number Confirmation
    Task<Result> GenerateEmailConfirmationToken(Guid userId);
    Task<Result> ConfirmEmail(Guid userId, string token);
    Task<Result> IsEmailConfirmed(Guid userId);
    Task<Result> ConfirmPhoneNumber(Guid userId, string code);
    Task<Result> IsPhoneNumberConfirmed(Guid userId);

    // Roles
    Task<Result> GetAllRoles();
    Task<Result<IEnumerable<RoleDto>>> GetRolesByUserId(Guid userId);
    Task<Result> GetRoleById(string roleId);
    Task<Result> CreateRole(RoleDto roleDto);
    Task<Result> UpdateRole(RoleDto roleDto);
    Task<Result> DeleteRole(string roleId);
    Task<Result> IsInRole(Guid userId, string role);

    // Permissions
    Task<Result<IEnumerable<string>>> GetPermissions(Guid userId);
    Task<Result> HasPermission(Guid userId, string permission);

    // Password Reset
    Task<Result> GeneratePasswordResetToken(Guid userId);
    Task<Result> ResetPassword(Guid userId, string token, string newPassword);
    Task<Result> ChangePassword(Guid userId, string currentPassword, string newPassword);
}