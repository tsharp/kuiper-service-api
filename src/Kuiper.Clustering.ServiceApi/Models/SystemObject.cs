using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace Kuiper.Clustering.ServiceApi.Models
{
    [DataContract]
    public class SystemObject<TData> : ISystemObject
        where TData : class, new()
    {
        [DataMember(Order = 0)]
        [YamlMember(Order = 0)]
        public required string ApiVersion { get; set; }

        [DataMember(Order = 1)]
        [YamlMember(Order = 1)]
        public required string Kind { get; set; }

        [DataMember(Order = 2)]
        [YamlMember(Order = 2)]
        public SystemObjectMetadata? Metadata { get; set; }

        [DataMember(Order = 3)]
        [YamlMember(Order = 3)]
        public string? Type { get; set; }

        [DataMember(Order = 4)]
        [YamlMember(Order = 4)]
        public TData? Data { get; set; }
    }
}
