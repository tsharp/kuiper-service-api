using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Kuiper.ServiceInfra.Configuration.Core;

internal sealed class KuiperConfigurationProvider : IConfigurationProvider
{
    public KuiperConfigurationProvider(string connectionString)
    {
    }

    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        throw new System.NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void Set(string key, string? value)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGet(string key, out string? value)
    {
        throw new System.NotImplementedException();
    }
}
