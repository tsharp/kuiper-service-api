using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Kuiper.Clustering.ServiceApi.Models;

namespace Kuiper.Clustering.ServiceApi.Dto
{
    [DataContract]
    public class SystemObjectBase<TMetadata> : ISystemObject
        where TMetadata : SystemObjectMetadata
    {
        [DataMember(Order = 0)]
        [JsonPropertyOrder(0)]
        public required string ApiVersion { get; set; }

        [DataMember(Order = 10)]
        [JsonPropertyOrder(1)]
        public virtual required string Kind { get; set; }

        [DataMember(Order = 20)]
        [JsonPropertyOrder(20)]
        public required SystemObjectMetadata Metadata { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object>? Properties { get; set; }
    }
}
