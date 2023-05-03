namespace MyCompany.MyProduct.Application.Abstractions.Common;

public interface IDateTime
{
    DateTime UtcNow { get; }
}