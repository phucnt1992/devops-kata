using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;

namespace DevOpsKata.Commands;
internal class SslDetectCommand : Command
{
    public SslDetectCommand() : base("ssl-detect", "Detects SSL certificate expiration")
    {
        AddOption(new Option<string>(new[] { "--domain", "-d" }, "Domain to check"));
        AddOption(new Option<bool>(new[] { "--verbose", "-v" }, "Verbose output"));

        Handler = CommandHandler.Create<ParseResult, SslDetectOptions, CancellationToken>((result, Options, token) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices()
                .UseSerilog()
                .R

    }

    protected override void
}
