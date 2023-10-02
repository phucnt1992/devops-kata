using Microsoft.Extensions.Logging;

namespace DevOpsKata.Infrastructure.Client;

public class SslHttpClient
{
    private readonly ILogger<SslHttpClient> _logger;

    public SslHttpClient(ILogger<SslHttpClient> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;
    }

    public async Task<HttpResponseMessage> DetectSslExpiry(string url, int thresholdDate)
    {
        // Create an HttpClientHandler object and set to use default credentials
        using var handler = new HttpClientHandler();

        // Set custom server validation callback
        handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, x509Chain, SslPolicyErrors) =>
        {
            _logger.LogDebug("Requested URI: {RequestUri}", requestMessage.RequestUri);

            if (certificate is null)
                return false;

            _logger.LogDebug("Issuer: {Issuer}", certificate.Issuer);
            _logger.LogDebug("Subject: {Subject}", certificate.Subject);

            var certExpirationDateString = certificate.GetExpirationDateString();
            if (!DateTime.TryParse(certExpirationDateString, out var certExpirationDate))
            {
                _logger.LogError("Unable to parse certificate expiration date: {CertExpirationDateString}", certExpirationDateString);
                return false;
            }

            if (certExpirationDate < DateTime.Now.AddDays(thresholdDate))
            {
                _logger.LogError("Certificate expiration date is less than threshold date: {CertExpirationDate}", certExpirationDate);
                return false;
            }

            return true;
        };

        // Create an HttpClient object
        using var client = new HttpClient(handler);

        return await client.GetAsync(url);
    }
}
