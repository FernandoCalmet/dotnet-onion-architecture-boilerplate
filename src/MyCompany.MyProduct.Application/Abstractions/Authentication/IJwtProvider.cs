namespace MyCompany.MyProduct.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(Guid userId, string userEmail);
}