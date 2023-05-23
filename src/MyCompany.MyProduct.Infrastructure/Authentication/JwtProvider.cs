using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyCompany.MyProduct.Application.Abstractions.Authentication;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCompany.MyProduct.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IUserService _userService;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IUserService userService)
    {
        _jwtOptions = jwtOptions.Value;
        _userService = userService;
    }

    public async Task<string> Generate(UserDto user)
    {
        var roles = await _userService.GetRolesByUserId(user.Id);
        var permissions = await _userService.GetPermissions(user.Id);
        var claims = CreateClaims(user, roles.Value, permissions.Value);
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
}