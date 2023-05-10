namespace MyCompany.MyProduct.Infrastructure.Logging;

public class LoggerOptions
{
    public string AppName { get; set; } = string.Empty;
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}