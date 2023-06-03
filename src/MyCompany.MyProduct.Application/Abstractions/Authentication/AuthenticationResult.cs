namespace MyCompany.MyProduct.Application.Abstractions.Authentication;

public sealed record AuthenticationResult(string AccessToken, string? RefreshToken);