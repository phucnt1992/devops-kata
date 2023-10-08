using Microsoft.Extensions.Hosting;
using DevOpsKata.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsKata.Common
{
    internal class CliRunner : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ResultCodeAccessor _resultCode;

        public CliRunner(
            IServiceProvider services,
            IHostApplicationLifetime lifetime,
            ResultCodeAccessor errorCode,
            ILogger<CliRunner> logger)
        {
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _resultCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Configuration loaded from context base directory: {BaseDirectory}", AppContext.BaseDirectory);
                using var scope = _services.CreateScope();

                await scope.ServiceProvider.GetRequiredService<IDevOpsKataCommand>().ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the CLI.");
                _resultCode.SetResultCode(ResultCodes.UnexpectedError);
            }
            finally
            {
                _lifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
