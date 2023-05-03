using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Identity;
using MyCompany.MyProduct.Infrastructure.Persistence;

namespace MyCompany.MyProduct.Infrastructure.Identity;

internal static class DependencyInjection
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        AddScopedUserService(services);
        ConfigureIdentityCore(services);
        ConfigureIdentityPasswordOptions(services);
        ConfigureIdentityLockoutOptions(services);
        ConfigureIdentityTwoFactorOptions(services);
    }

    private static void AddScopedUserService(IServiceCollection services) =>
        services.AddScoped<IUserService, UserService>();

    private static void ConfigureIdentityCore(IServiceCollection services) =>
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, typeof(GuidAuthenticatorTokenProvider));

    private static void ConfigureIdentityPasswordOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 4;
        });

    private static void ConfigureIdentityLockoutOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        });

    private static void ConfigureIdentityTwoFactorOptions(IServiceCollection services) =>
        services.Configure<IdentityOptions>(options =>
        {
            options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
        });
}