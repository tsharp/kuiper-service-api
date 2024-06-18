using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi.Models
{
    public class SystemObjectMetadata
    {
        public string Name { get; set; }

        public IDictionary<string, object> Annotations { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}
