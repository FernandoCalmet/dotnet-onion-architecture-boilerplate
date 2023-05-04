using Microsoft.OpenApi.Models;
using MyCompany.MyProduct.Application;
using MyCompany.MyProduct.Infrastructure;
using MyCompany.MyProduct.Presentation;

namespace MyCompany.MyProduct.WebApi.Configuration;

public static class DependencyInjection
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        AddApplicationServices(builder);
        AddPresentationServices(builder);
        AddHealthChecks(builder);
        ConfigureSwagger(builder);
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            UseSwagger(app);
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseHealthChecks("/health");
    }

    private static void AddApplicationServices(WebApplicationBuilder builder) =>
        builder.Services
            .AddInfrastructureServices(builder.Configuration)
            .AddApplicationServices();

    private static void AddPresentationServices(WebApplicationBuilder builder) =>
        builder.Services.AddPresentationServices();

    private static void AddHealthChecks(WebApplicationBuilder builder) =>
        builder.Services.AddHealthChecks();

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MyCompany : MyProduct API",
                Version = "v1"
            });

            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
            });
        });
    }

    private static void UseSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}