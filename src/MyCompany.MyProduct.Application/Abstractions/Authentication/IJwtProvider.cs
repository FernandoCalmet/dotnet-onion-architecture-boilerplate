using MyCompany.MyProduct.Application.Abstractions.Identity;

namespace MyCompany.MyProduct.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> Generate(UserDto user);
}