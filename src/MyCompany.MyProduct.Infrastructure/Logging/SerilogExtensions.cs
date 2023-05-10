using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace MyCompany.MyProduct.Infrastructure.Logging;

internal static class SerilogExtensions
{
    internal static WebApplicationBuilder RegisterSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<LoggerOptions>().BindConfiguration(nameof(LoggerOptions));

        _ = builder.Host.UseSerilog((_, serviceProvider, serilogConfig) =>
        {
            var loggerSettings = serviceProvider.GetRequiredService<IOptions<LoggerOptions>>().Value;
            var appName = loggerSettings.AppName;
            var elasticSearchUrl = loggerSettings.ElasticSearchUrl;
            var writeToFile = loggerSettings.WriteToFile;
            var structuredConsoleLogging = loggerSettings.StructuredConsoleLogging;
            var minLogLevel = loggerSettings.MinimumLogLevel;
            ConfigureEnrichers(serilogConfig, appName);
            ConfigureConsoleLogging(serilogConfig, structuredConsoleLogging);
            ConfigureWriteToFile(serilogConfig, writeToFile);
            ConfigureElasticSearch(builder, serilogConfig, appName, elasticSearchUrl);
            SetMinimumLogLevel(serilogConfig, minLogLevel);
            OverrideMinimumLogLevel(serilogConfig);
            Console.WriteLine(FiggleFonts.Standard.Render(loggerSettings.AppName));
        });

        return builder;
    }

    private static void ConfigureEnrichers(LoggerConfiguration serilogConfig, string appName) =>
        serilogConfig
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", appName)
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId();

    private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool structuredConsoleLogging)
    {
        if (structuredConsoleLogging)
        {
            serilogConfig.WriteTo.Async(sink => sink.Console(new CompactJsonFormatter()));
        }
        else
        {
            serilogConfig.WriteTo.Async(sink => sink.Console());
        }
    }

    private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile)
    {
        if (writeToFile)
        {
            serilogConfig.WriteTo.File(
                new CompactJsonFormatter(),
                "Logs/logs.json",
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 5);
        }
    }

    private static void ConfigureElasticSearch(
        WebApplicationBuilder builder,
        LoggerConfiguration serilogConfig,
        string appName,
        string elasticSearchUrl)
    {
        if (string.IsNullOrEmpty(elasticSearchUrl)) return;

        var formattedAppName = FormatAppNameForElasticSearch(appName);
        var indexFormat = CreateElasticSearchIndexFormat(formattedAppName, builder.Environment.EnvironmentName);

        serilogConfig.WriteTo.Async(writeTo =>
            writeTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
            {
                AutoRegisterTemplate = true,
                IndexFormat = indexFormat,
                MinimumLogEventLevel = LogEventLevel.Information,
            })).Enrich.WithProperty("Environment", builder.Environment.EnvironmentName!);
    }

    private static string? FormatAppNameForElasticSearch(string appName) =>
        appName?.ToLower().Replace(".", "-").Replace(" ", "-");

    private static string CreateElasticSearchIndexFormat(string formattedAppName, string environmentName)
    {
        var formattedEnvironmentName = environmentName?.ToLower().Replace(".", "-");
        return $"{formattedAppName}-logs-{formattedEnvironmentName}-{DateTime.UtcNow:yyyy-MM}";
    }

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        switch (minLogLevel.ToLower())
        {
            case "debug":
                serilogConfig.MinimumLevel.Debug();
                break;
            case "information":
                serilogConfig.MinimumLevel.Information();
                break;
            case "warning":
                serilogConfig.MinimumLevel.Warning();
                break;
            default:
                serilogConfig.MinimumLevel.Information();
                break;
        }
    }

    private static void OverrideMinimumLogLevel(LoggerConfiguration serilogConfig) =>
        serilogConfig
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
}