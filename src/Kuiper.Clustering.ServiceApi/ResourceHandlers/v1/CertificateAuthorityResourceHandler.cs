
using Kuiper.Clustering.ServiceApi.Dto;
using Kuiper.Clustering.ServiceApi.Resources;
using Kuiper.Clustering.ServiceApi.Security;
using Kuiper.Clustering.ServiceApi.Storage;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers.v1
{
    [ResourceType("security.kuiper-sys.com", "v1", "CertificateAuthority")]
    public class CertificateAuthorityResourceHandler : ResourceServiceHandlerBase<CertificateAuthority>
    {
        protected override string ResourceKindName => "CertificateAuthority";

        public CertificateAuthorityResourceHandler(IKeyValueStore configStore) : base(configStore)
        {
        }

        protected override async Task<IResult> HandlePostRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(resourcePathDescriptor.SubResourcePath))
            {
                return Results.BadRequest(new
                {
                    error = new
                    {
                        message = "Resource action name must be declared."
                    }
                });
            }

            switch (resourcePathDescriptor.SubResourcePath.ToLowerInvariant())
            {
                case "issue/self-signed":
                    return await this.InitializeInternalCA(httpContext, resourcePathDescriptor, cancellationToken);
                default:
                    return Results.BadRequest(new
                    {
                        error = new
                        {
                            message = "Unknown resource action."
                        }
                    });
            }
        }

        private async Task<IResult> InitializeInternalCA(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            var friendlyName = "Kuiper Clustering Service";

            var certificate = CertificateGenerator.SelfSignedCertificateAuthority(
                friendlyName,
                "CN=Kuiper CA,OU=Kuiper Clustering Service,O=Kuiper Microsystems,C=US",
                30);

            var certificateAuthority = new CertificateAuthority
            {
                ApiVersion = "security.kuiper-sys.com/v1",
                Type = "CertificateAuthority",
                Kind = "Secret",
                Metadata = new SystemObjectMetadata()
                {
                    Name = resourcePathDescriptor.ResourceName,
                    Namespace = "kuiper-system"
                },
                Data = new CertificateAuthorityData
                {
                    FriendlyName = friendlyName,
                    EnableDiscovery = true,
                    Certificate = certificate.EncodedPublicKey,
                    PrivateKey = certificate.EncodedPrivateKey,
                }
            };

            var result = await this.configStore.SetAsync(resourcePathDescriptor.ResourceId, certificateAuthority, cancellationToken);

            return Results.Created(resourcePathDescriptor.ResourceId, result);
        }
    }
}
