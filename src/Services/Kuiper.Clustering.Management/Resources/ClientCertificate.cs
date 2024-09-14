using Kuiper.Clustering.ServiceApi.Models;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi.Resources
{
    public class ClientCertificate : SecretObject<CertificateData>
    {
        public ClientCertificate() : base()
        {
            Type = nameof(ClientCertificate);
        }

        [DataMember(Order = 11)]
        [JsonPropertyOrder(11)]
        public override required string Type { get; set; } = nameof(ClientCertificate);
    }
}
