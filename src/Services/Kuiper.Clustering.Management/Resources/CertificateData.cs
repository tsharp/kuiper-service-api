using Kuiper.Clustering.ServiceApi.Security;
using System.Runtime.Serialization;

namespace Kuiper.Clustering.ServiceApi.Resources
{
    [DataContract]
    public class CertificateData
    {
        [DataMember]
        public string? FriendlyName { get; set; }

        [DataMember]
        public string? Certificate { get; set; }

        [SecretData]
        public string? PrivateKey { get; set; }
    }
}
