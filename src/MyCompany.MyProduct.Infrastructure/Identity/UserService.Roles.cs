using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        return (await _roleManager.Roles.ToListAsync()).Adapt<IEnumerable<RoleDto>>();
    }

    public async Task<RoleDto> GetRoleByIdAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        return role.Adapt<RoleDto>();
    }

    public async Task CreateRoleAsync(RoleDto roleDto)
    {
        var role = roleDto.Adapt<ApplicationRole>();
        await _roleManager.CreateAsync(role);
    }

    public async Task UpdateRoleAsync(RoleDto roleDto)
    {
        var role = await _roleManager.FindByIdAsync(roleDto.Id);
        roleDto.Adapt(role);
        await _roleManager.UpdateAsync(role);
    }

    public async Task DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        await _roleManager.DeleteAsync(role);
    }

    public async Task<bool> IsInRoleAsync(UserDto user, string role)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return await _userManager.IsInRoleAsync(appUser, role);
    }
}