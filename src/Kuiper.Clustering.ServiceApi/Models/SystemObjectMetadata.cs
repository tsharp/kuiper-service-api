using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Kuiper.Clustering.ServiceApi.Models
{
    public class SystemObjectMetadata
    {
        public required string Name { get; set; }

        public required string Namespace { get; set; }

        public IDictionary<string, string>? Labels { get; set; }

        public IDictionary<string, object>? Annotations { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object>? Properties { get; set; }
    }
}
