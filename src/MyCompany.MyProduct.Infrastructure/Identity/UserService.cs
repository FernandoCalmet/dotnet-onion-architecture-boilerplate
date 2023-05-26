using Mapster;
using Microsoft.AspNetCore.Identity;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Application.Exceptions;
using MyCompany.MyProduct.Core.Shared;
using static MyCompany.MyProduct.Infrastructure.Identity.IdentityErrors;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> CreateUser(UserDto user, string password)
    {
        try
        {
            var appUser = new ApplicationUser
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
            };

            var hashedPassword = _userManager.PasswordHasher.HashPassword(appUser, password);

            var result = await _userManager.CreateAsync(appUser, hashedPassword);
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }
        catch
        {
            throw new ConflictException($"An error occurred while finding the user by email: {user.Email}.");
        }
    }

    public async Task<Result> UpdateUser(UserDto user)
    {
        try
        {
            var appUser = await GetUserById(user.Id);

            appUser.Value.Email = user.Email;
            appUser.Value.PhoneNumber = user.PhoneNumber;

            var result = await _userManager.UpdateAsync(appUser.Value);
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }
        catch
        {
            throw new ConflictException($"An error occurred while updating the user by ID: {user.Id}.");
        }
    }

    public async Task<Result> DeleteUser(Guid userId)
    {
        try
        {
            var user = await GetUserById(userId);

            var result = await _userManager.DeleteAsync(user.Value);
            var errors = result.Errors.Select(x => new Error(x.Code, x.Description));

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }
        catch
        {
            throw new ConflictException($"An error occurred while deleting the user by ID: {userId}.");
        }
    }

    public async Task<Result<UserDto>> FindUserById(Guid userId)
    {
        try
        {
            var appUser = await GetUserById(userId);

            var user = appUser.Value.Adapt<UserDto>();

            return Result.Success(user);
        }
        catch
        {
            throw new NotFoundException($"An error occurred while finding the user by ID: {userId}.");
        }
    }

    public async Task<Result<UserDto>> FindUserByEmail(string email)
    {
        try
        {
            Maybe<ApplicationUser> maybeUser = await _userManager.FindByEmailAsync(email) ?? null!;
            if (maybeUser.HasNoValue)
            {
                return Result.Failure<UserDto>(Account.UserNotFound);
            }

            var user = maybeUser.Value.Adapt<UserDto>();
            return Result.Success(user);
        }
        catch
        {
            throw new NotFoundException($"An error occurred while finding the user by email: {email}.");
        }
    }

    public async Task<Result> IsEmailUnique(string email)
    {
        try
        {
            Maybe<ApplicationUser> maybeUser = await _userManager.FindByEmailAsync(email) ?? null!;
            if (maybeUser.HasNoValue)
            {
                return Result.Failure(Account.UserNotFound);
            }

            return maybeUser.Value.Email == email
                ? Result.Failure<bool>(Account.EmailAlreadyExists)
                : Result.Success();
        }
        catch
        {
            throw new ConflictException($"An error occurred while checking if the email {email} is unique.");
        }
    }

    public async Task<Result> CheckPassword(Guid userId, string password)
    {
        try
        {
            var user = await GetUserById(userId);

            var hashedPassword = _userManager.PasswordHasher.HashPassword(user.Value, password);
            var isPasswordValid = await IsPasswordValid(user.Value, hashedPassword);

            return isPasswordValid
                ? Result.Success(isPasswordValid)
                : Result.Failure<bool>(Account.InvalidPassword);
        }
        catch
        {
            throw new ConflictException($"An error occurred while checking the password of the user by ID: {userId}.");
        }
    }

    public async Task<Result> HasPassword(Guid userId)
    {
        try
        {
            var user = await GetUserById(userId);

            var hasPassword = await _userManager.HasPasswordAsync(user.Value);

            return hasPassword
                ? Result.Success(hasPassword)
                : Result.Failure<bool>(Account.InvalidPassword);
        }
        catch
        {
            throw new ConflictException($"An error occurred while checking user by ID {userId} has password.");
        }
    }

    private async Task<Result<ApplicationUser>> GetUserById(Guid userId)
    {
        try
        {
            Maybe<ApplicationUser> maybeUser = await _userManager.FindByIdAsync(userId.ToString()) ?? null!;
            if (maybeUser.HasNoValue)
            {
                return Result.Failure<ApplicationUser>(Account.UserNotFound);
            }

            var user = maybeUser.Value;
            return Result.Success(user);
        }
        catch
        {
            throw new NotFoundException($"An error occurred while finding the user by ID: {userId}.");
        }
    }

    private async Task<bool> IsPasswordValid(ApplicationUser user, string password)
    {
        try
        {
            var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }
        catch
        {
            throw new ConflictException($"An error occurred while checking if the password is valid of the user by ID: {user.Id}.");
        }
    }
}