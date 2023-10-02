namespace DevOpsKata.Infrastructure;
using System.CommandLine.Binding;
using Microsoft.Extensions.Logging;
using Serilog;

public class LoggerBinder : BinderBase<Microsoft.Extensions.Logging.ILogger>
{
    protected override Microsoft.Extensions.Logging.ILogger GetBoundValue(BindingContext context)
    {
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog();

        return loggerFactory.CreateLogger("DevOpsKata");
    }
}
