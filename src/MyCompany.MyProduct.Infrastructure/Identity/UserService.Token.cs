using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Exceptions;
using MyCompany.MyProduct.Core.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    public async Task<AuthenticationResult> GenerateToken(UserDto user)
    {
        try
        {
            var roles = await GetRolesByUserId(user.Id);
            var permissions = await GetPermissions(user.Id);

            var jwtToken = CreateJwtToken(user, roles.Value, permissions.Value);

            var refreshToken = GenerateRefreshToken();

            var result = new AuthenticationResult(jwtToken, refreshToken);
            return result;
        }
        catch
        {
            throw new ConflictException($"An error occurred while generating the token for user: {user.Email}.");
        }
    }

    public async Task<Result<AuthenticationResult>> RefreshToken(string refreshToken)
    {
        try
        {
            var appUser = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (appUser is null)
            {
                return Result.Failure<AuthenticationResult>(new Error("InvalidToken", "The refresh token is invalid."));
            }

            var user = new UserDto { Id = appUser.Id, Email = appUser.Email! };

            var newJwtToken = await GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await SetRefreshToken(user, newRefreshToken);

            return Result.Success(new AuthenticationResult(newJwtToken.AccessToken, newRefreshToken));
        }
        catch
        {
            throw new ConflictException("An error occurred while refreshing the token.");
        }
    }

    private async Task SetRefreshToken(UserDto user, string refreshToken)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser != null)
        {
            appUser.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(appUser);
        }
    }

    private string CreateJwtToken(UserDto user, IEnumerable<RoleDto> roles, IEnumerable<string> permissions)
    {
        var claims = CreateClaims(user, roles, permissions);
        var signingCredentials = CreateSigningCredentials();
        var token = CreateJwtSecurityToken(claims, signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static IEnumerable<Claim> CreateClaims(UserDto user, IEnumerable<RoleDto> roles, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString() ?? null!)));
        claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken CreateJwtSecurityToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials) =>
        new(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}