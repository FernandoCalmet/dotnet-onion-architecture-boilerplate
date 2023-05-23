using Mapster;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<Result> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var listRoles = roles.Adapt<IEnumerable<RoleDto>>();
        return Result.Success(listRoles);
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetRolesByUserId(Guid userId)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure<IEnumerable<RoleDto>>(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var roles = await _userManager.GetRolesAsync(user);
        var listRoles = roles.Adapt<IEnumerable<RoleDto>>();
        return Result.Success(listRoles);
    }

    public async Task<Result> GetRoleById(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        return role is null
            ? Result.Failure(Role.NotFound)
            : Result.Success(role.Adapt<RoleDto>());
    }

    public async Task<Result> CreateRole(RoleDto roleDto)
    {
        var role = roleDto.Adapt<ApplicationRole>();
        var identityResult = await _roleManager.CreateAsync(role);
        var result = Result.Create(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> UpdateRole(RoleDto roleDto)
    {
        var role = await _roleManager.FindByIdAsync(roleDto.Id);
        if (role is null)
        {
            return Result.Failure(Role.NotFound);
        }

        roleDto.Adapt(role);
        var identityResult = await _roleManager.UpdateAsync(role);
        var result = Result.Create(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> DeleteRole(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
        {
            return Result.Failure(Role.NotFound);
        }

        var identityResult = await _roleManager.DeleteAsync(role);
        var result = Result.Create(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(result.Error);
    }

    public async Task<Result> IsInRole(Guid userId, string role)
    {
        Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
        if (maybeUser.HasNoValue)
        {
            return Result.Failure(Account.UserNotFound);
        }

        var user = maybeUser.Value;
        var isInRole = await _userManager.IsInRoleAsync(user, role);
        var result = Result.Create(string.Join(", ", isInRole));

        return isInRole
            ? Result.Success(isInRole)
            : Result.Failure(result.Error);
    }
}