using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Plaform.ManagementObjects.Cluster;

public class KuiperCluster : SystemObject
{
    [DataMember(Order = int.MaxValue)]
    [JsonPropertyOrder(int.MaxValue)]
    public ClusterData Data { get; set; }
}
