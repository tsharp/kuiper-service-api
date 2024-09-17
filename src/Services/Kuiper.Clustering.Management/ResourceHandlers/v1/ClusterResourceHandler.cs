using Kuiper.Plaform.ManagementObjects.Cluster;
using Kuiper.Plaform.ServiceApi;
using Kuiper.Plaform.ServiceApi.ResourceHandlers;
using Kuiper.ServiceInfra.Persistence;

namespace Kuiper.Clustering.Management.ResourceHandlers.v1;

[ResourceType("security.kuiper-sys.com", "v1", "Cluster")]
public class ClusterResourceHandler : ResourceServiceHandlerBase<KuiperCluster>
{
    public ClusterResourceHandler(IKeyValueStore configStore) : base(configStore)
    {
    }

    protected override string ResourceKindName => "Cluster";
}
