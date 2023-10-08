using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DevOpsKata.Common.Extensions;

using Exceptionless.DateTimeExtensions;

namespace DevOpsKata.SslExpiryDetector.Commands;

public class SslDetectCommand : Command
{
    public SslDetectCommand() : base("ssl-detect", "Detects SSL certificate expiration")
    {
        Handler = CommandHandler.Create<ParseResult, SslDetectCommandOptions, CancellationToken>((result, options, token) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(options);
                })
                .RunCliCommandAsync(token)
        );

        AddArgument(new Argument<string>("domain", "Domain to check for SSL certificate expiration"));
        AddArgument(new Argument<TimeSpan>("threshold", parse: (value) =>
        {
            if (value?.Tokens is null || !value.Tokens.Any())
            {
                throw new ArgumentException(@"Must specify threshold in the format of ""1d 2h 3m 4s""");
            }

            return TimeUnit.Parse(value.Tokens.ToString());
        }, description: "Threshold for SSL certificate expiration"));

        AddOption(new Option<bool>(new[] { "--verbose", "-v" }, "Verbose output"));
    }
}
