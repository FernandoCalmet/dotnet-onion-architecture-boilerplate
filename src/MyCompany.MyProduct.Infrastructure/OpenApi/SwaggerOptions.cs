namespace MyCompany.MyProduct.Infrastructure.OpenApi;

public class SwaggerOptions
{
    public bool Enable { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactUrl { get; set; }
    public bool License { get; set; }
    public string? LicenseName { get; set; }
    public string? LicenseUrl { get; set; }
    public string? SecurityName { get; set; }
    public string? SecurityScheme { get; set; }
}