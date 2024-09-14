using Kuiper.Clustering.Management.Security;
using System.Runtime.Serialization;

namespace Kuiper.Clustering.Management.Resources
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
