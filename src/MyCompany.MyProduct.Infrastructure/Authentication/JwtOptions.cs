namespace MyCompany.MyProduct.Infrastructure.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
}