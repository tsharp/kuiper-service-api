using Kuiper.Clustering.ServiceApi.Models;

namespace Kuiper.Clustering.ServiceApi.Resources
{
    public abstract class ResourceDescriptor<T> : SystemObject<T>
        where T : class, new()
    {
    }
}
