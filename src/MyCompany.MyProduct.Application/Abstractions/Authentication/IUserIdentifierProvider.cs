namespace MyCompany.MyProduct.Application.Abstractions.Authentication;

public interface IUserIdentifierProvider
{
    Guid UserId { get; }
}