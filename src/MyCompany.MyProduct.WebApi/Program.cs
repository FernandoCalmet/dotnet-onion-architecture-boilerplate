using MyCompany.MyProduct.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder.Build();
app.ConfigureMiddleware();

app.Run();