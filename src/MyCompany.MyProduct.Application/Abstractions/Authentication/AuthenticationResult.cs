namespace MyCompany.MyProduct.Application.Abstractions.Authentication;

public sealed class AuthenticationResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
}