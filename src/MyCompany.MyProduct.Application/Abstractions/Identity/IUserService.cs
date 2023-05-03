namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public interface IUserService
{
    // User
    Task<UserDto> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(UserDto user, string password);
    Task<bool> HasPasswordAsync(UserDto user);

    // Two-Factor Authentication
    Task EnableTwoFactorAuthenticationAsync(UserDto user);
    Task DisableTwoFactorAuthenticationAsync(UserDto user);
    Task<bool> IsTwoFactorAuthenticationEnabledAsync(UserDto user);
    Task<bool> HasAuthenticatorAsync(UserDto user);

    // Account Lockout
    Task LockAccountAsync(UserDto user);
    Task UnlockAccountAsync(UserDto user);
    Task<bool> IsLockedOutAsync(UserDto user);

    // Email & Phone Number Confirmation
    Task<string> GenerateEmailConfirmationTokenAsync(UserDto user);
    Task<string> ConfirmEmailAsync(UserDto user, string token);
    Task<bool> IsEmailConfirmedAsync(UserDto user);
    Task<string> ConfirmPhoneNumberAsync(UserDto user, string code);
    Task<bool> IsPhoneNumberConfirmedAsync(UserDto user);

    // Roles
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto> GetRoleByIdAsync(string roleId);
    Task CreateRoleAsync(RoleDto roleDto);
    Task UpdateRoleAsync(RoleDto roleDto);
    Task DeleteRoleAsync(string roleId);
    Task<bool> IsInRoleAsync(UserDto user, string role);

    // Permissions
    Task<List<string>> GetPermissionsAsync(UserDto user);
    Task<bool> HasPermissionAsync(UserDto user, string permission);

    // Password Reset
    Task<string> GeneratePasswordResetTokenAsync(UserDto user);
    Task<string> ResetPasswordAsync(UserDto user, string token, string newPassword);
}