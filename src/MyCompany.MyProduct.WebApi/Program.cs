using MyCompany.MyProduct.Application;
using MyCompany.MyProduct.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services
        .AddInfrastructureServices(builder.Configuration)
        .AddApplicationServices();

    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

static void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseAuthentication();
    app.MapControllers();
    app.UseHealthChecks("/health");
}