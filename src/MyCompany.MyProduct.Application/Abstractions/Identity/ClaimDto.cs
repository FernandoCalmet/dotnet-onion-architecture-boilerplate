namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public class ClaimDto
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}