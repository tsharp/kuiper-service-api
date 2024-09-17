using Microsoft.Extensions.Configuration;

namespace Kuiper.ServiceInfra.Configuration.Core;

internal sealed class KuiperConfigurationSource : IConfigurationSource
{
    private string _connectionString { get; }

    public KuiperConfigurationSource(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new KuiperConfigurationProvider(_connectionString);
}
