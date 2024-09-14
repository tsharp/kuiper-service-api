using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Kuiper.Clustering.ServiceApi.Dto;

namespace Kuiper.Clustering.ServiceApi.Models
{
    public class SecretObject<TData> : SystemObject
    {
        public SecretObject() : base()
        {
            Kind = "Secret";
        }

        [DataMember(Order = 10)]
        [JsonPropertyOrder(10)]
        public override required string Kind { get; set; } = "Secret";

        [DataMember(Order = 11)]
        [JsonPropertyOrder(11)]
        public required virtual string Type { get; set; }

        [DataMember(Order = int.MaxValue)]
        [JsonPropertyOrder(int.MaxValue)]
        public required TData Data { get; set; }
    }
}
