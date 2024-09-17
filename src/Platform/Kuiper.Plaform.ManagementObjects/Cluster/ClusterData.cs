using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Plaform.ManagementObjects.Cluster;

[DataContract]
public class ClusterData
{
    [JsonExtensionData]
    public IDictionary<string, object>? Properties { get; set; }
}
