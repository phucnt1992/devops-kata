using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using DevOpsKata.SslExpiryDetector.Commands;

static string GetProcessName()
{
    using var current = System.Diagnostics.Process.GetCurrentProcess();
    return current.ProcessName;
}

var root = new RootCommand
{
    // Get name from process so that it will show correctly if run as a .NET CLI tool
    Name = GetProcessName(),
};

root.AddCommand(new SslDetectCommand());

return await new CommandLineBuilder(root)
        .UseDefaults()
        .Build()
        .InvokeAsync(args);


