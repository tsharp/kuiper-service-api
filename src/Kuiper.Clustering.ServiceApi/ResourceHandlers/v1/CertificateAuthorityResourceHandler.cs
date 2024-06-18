
using Kuiper.Clustering.ServiceApi.Storage;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers.v1
{
    [ResourceType("security.kuiper-sys.com", "v1", "CertificateAuthority")]
    public class CertificateAuthorityResourceHandler : ResourceServiceHandlerBase
    {
        protected override string ResourceKindName => "CertificateAuthority";

        public CertificateAuthorityResourceHandler(IKeyValueStore configStore) : base(configStore)
        {
        }
    }
}
