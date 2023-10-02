using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace DevOpsKata.Commands;

public class SslDetectOptions
{
    private bool _isServicesConfigured;

    public string Domain { get; set; } = string.Empty;
    public bool Verbose { get; set; }

    public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        if (_isServicesConfigured)
        {
            return;
        }

        services.AddSingleton(this);

        _isServicesConfigured = true;
    }
}
