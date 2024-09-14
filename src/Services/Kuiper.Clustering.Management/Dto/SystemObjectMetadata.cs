using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi.Dto
{
    [DataContract]
    public class SystemObjectMetadata
    {
        [DataMember(Order = 0)]
        [JsonPropertyOrder(0)]
        public required string Name { get; set; }

        [DataMember(Order = 10)]
        [JsonPropertyOrder(10)]
        public required string Namespace { get; set; }

        [DataMember(Order = 20)]
        [JsonPropertyOrder(20)]
        public IDictionary<string, string>? Labels { get; set; }

        [DataMember(Order = 30)]
        [JsonPropertyOrder(30)]
        public IDictionary<string, object>? Annotations { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object>? Properties { get; set; }
    }
}
