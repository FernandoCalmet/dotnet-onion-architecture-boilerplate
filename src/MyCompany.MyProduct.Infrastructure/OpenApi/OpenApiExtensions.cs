using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MyCompany.MyProduct.Infrastructure.OpenApi;

internal static class OpenApiExtensions
{
    internal static IServiceCollection AddOpenApiDocumentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<SwaggerOptionsSetup>();
        services.ConfigureSwagger(configuration);

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    private static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(swaggerGenOptions =>
        {
            var swaggerOptions = configuration.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>()!;

            if (!swaggerOptions.Enable) return;

            swaggerGenOptions.SwaggerDoc(swaggerOptions.Name, new OpenApiInfo
            {
                Title = swaggerOptions.Title,
                Version = swaggerOptions.Version,
                Description = swaggerOptions.Description,
                Contact = new OpenApiContact
                {
                    Name = swaggerOptions.ContactName,
                    Email = swaggerOptions.ContactEmail,
                    Url = new Uri(swaggerOptions.ContactUrl)
                },
                License = swaggerOptions.License ? new OpenApiLicense
                {
                    Name = swaggerOptions.LicenseName,
                    Url = new Uri(swaggerOptions.LicenseUrl)
                } : null
            });

            swaggerGenOptions.AddSecurityDefinition(swaggerOptions.SecurityScheme, new OpenApiSecurityScheme
            {
                Description = swaggerOptions.Description,
                Name = swaggerOptions.SecurityName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = swaggerOptions.SecurityScheme
            });

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = swaggerOptions.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}