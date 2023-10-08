using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DevOpsKata.Common.Extensions;

public static class CommandExtensions
{

    public static IHostBuilder UseDevOpsKataCli<TApp>(this IHostBuilder host, IDevOpsKataOptions options)
            where TApp : class, IDevOpsKataCommand
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        return host
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<CliRunner>();
                services.AddSingleton(options);
                services.AddScoped<IDevOpsKataCommand, TApp>();
            })
            .UseSerilog((context, config) => config
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration)
            )
            .UseConsoleLifetime(options => options.SuppressStatusMessages = true);
    }

    public static async Task<int> RunCliCommandAsync(this IHostBuilder hostBuilder, CancellationToken token)
    {
        if (hostBuilder is null)
        {
            throw new ArgumentNullException(nameof(hostBuilder));
        }

        var host = hostBuilder.Build();

        var errorCode = host.Services.GetRequiredService<ErrorCodeAccessor>();

        try
        {
            await host.StartAsync(token).ConfigureAwait(false);

            await host.WaitForShutdownAsync(token).ConfigureAwait(false);
        }
        finally
        {
            if (host is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                host.Dispose();
            }
        }

        return errorCode.ErrorCode;
    }
}
