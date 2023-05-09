using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MyCompany.MyProduct.Infrastructure.OpenApi;

public class SwaggerOptionsSetup : IConfigureOptions<SwaggerOptions>
{
    private const string SectionName = nameof(SwaggerOptions);
    private readonly IConfiguration _configuration;

    public SwaggerOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SwaggerOptions options) => 
        _configuration.GetSection(SectionName).Bind(options);
}