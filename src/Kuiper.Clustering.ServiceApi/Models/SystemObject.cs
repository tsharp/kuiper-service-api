using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Kuiper.Clustering.ServiceApi.Models
{
    [DataContract]
    public class SystemObject : ISystemObject
    {
        [DataMember(Order = 0)]
        [YamlMember(Order = 0)]
        public required string ApiVersion { get; set; }

        [DataMember(Order = 1)]
        [YamlMember(Order = 1)]
        public required string Kind { get; set; }

        [DataMember(Order = 2)]
        [YamlMember(Order = 2)]
        public SystemObjectMetadata Metadata { get; set; } = new SystemObjectMetadata();

        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}
