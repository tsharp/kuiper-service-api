using Kuiper.Clustering.ServiceApi.Models;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi.Resources
{
    public class CertificateAuthority : SecretObject<CertificateAuthorityData>
    {
        public CertificateAuthority() : base()
        {
            Type = nameof(CertificateAuthority);
        }

        [DataMember(Order = 11)]
        [JsonPropertyOrder(11)]
        public override required string Type { get; set; } = nameof(CertificateAuthority);
    }
}
