using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using DevOpsKata.Infrastructure;
using Serilog;
using Serilog.Events;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Serilog", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();


// Setup the command line options
var urlOption = new Option<string>(
    new[] { "--url", "-u" },
    "The URL to check for SSL certificate expiration"
);

var thresholdOption = new Option<DateTime>(
    new[] { "--threshold", "-t" },
    "The date to check the SSL certificate expiration against"
);

var root = new RootCommand
{
    Name = "ssl-expiration-detection",
};

root.AddOption(urlOption);
root.AddOption(thresholdOption);

try
{
    await new CommandLineBuilder(root)
        .UseDefaults()
        .Build()
        .InvokeAsync(args);
}
finally
{
    // Flush the logger to ensure all log entries are written to the output
    Log.CloseAndFlush();
}
