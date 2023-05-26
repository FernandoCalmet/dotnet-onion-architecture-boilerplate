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
        var user = await GetUserById(userId);

        var roles = await _userManager.GetRolesAsync(user.Value);

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

        var result = await _roleManager.CreateAsync(role);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> UpdateRole(RoleDto roleDto)
    {
        var role = await _roleManager.FindByIdAsync(roleDto.Id);
        if (role is null)
        {
            return Result.Failure(Role.NotFound);
        }

        roleDto.Adapt(role);

        var result = await _roleManager.UpdateAsync(role);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> DeleteRole(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
        {
            return Result.Failure(Role.NotFound);
        }

        var result = await _roleManager.DeleteAsync(role);
        var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(errors);
    }

    public async Task<Result> IsInRole(Guid userId, string role)
    {
        var user = await GetUserById(userId);

        var isInRole = await _userManager.IsInRoleAsync(user.Value, role);

        var error = new Error("IsInRole", $"User {userId} is not in role {role}.");

        return isInRole
            ? Result.Success(isInRole)
            : Result.Failure(error);
    }
}