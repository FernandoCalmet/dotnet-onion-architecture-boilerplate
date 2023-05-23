using Microsoft.AspNetCore.Http;
using MyCompany.MyProduct.Application.Abstractions.Authentication;
using System.Security.Claims;

namespace MyCompany.MyProduct.Infrastructure.Authentication;

internal sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    public Guid UserId { get; }

    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        UserId = ExtractUserIdFromContext(httpContextAccessor);
    }

    private static Guid ExtractUserIdFromContext(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            throw new InvalidOperationException("Account identifier not found.");
        }

        return Guid.Parse(userId);
    }
}
