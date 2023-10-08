using Microsoft.Extensions.Logging;

using DevOpsKata.Common;
using DevOpsKata.Infrastructure.Client;

namespace DevOpsKata.SslExpiryDetector.Commands;

public class SslDetect : IDevOpsKataCommand
{
    private readonly SslHttpClient _client;
    private readonly SslDetectCommandOptions _options;
    private readonly ILogger<SslDetect> _logger;

    public SslDetect(SslHttpClient client, SslDetectCommandOptions options, ILogger<SslDetect> logger)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _client = client;
        _options = options;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            _logger.LogDebug("Detecting SSL expiry for {Domain}", _options.Domain);
            await _client.DetectSslExpiryAsync(_options.Domain, _options.Threshold.Days, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while detecting SSL expiry for {Domain}", _options.Domain);
            throw;
        }
    }
}
