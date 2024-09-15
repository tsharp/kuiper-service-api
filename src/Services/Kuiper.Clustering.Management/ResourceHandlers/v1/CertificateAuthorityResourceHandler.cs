//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Kuiper.Clustering.Management.Security;
using Kuiper.Plaform.ManagementObjects;
using Kuiper.Plaform.ManagementObjects.Secrets;
using Kuiper.Plaform.ServiceApi;
using Kuiper.Plaform.ServiceApi.ResourceHandlers;
using Kuiper.ServiceInfra.Persistence;
using Kuiper.ServiceInfra.Security.Certificates;
using System.Security.Cryptography.X509Certificates;

namespace Kuiper.Clustering.Management.ResourceHandlers.v1;

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

        var subResourcePath = resourcePathDescriptor.SubResourcePath.ToLowerInvariant();
        var subResourcePathParts = subResourcePath.Split("/");

        if (subResourcePathParts.Length != 2)
        {
            return Results.BadRequest(new
            {
                error = new
                {
                    message = "Invalid subresource path."
                }
            });
        }

        var subResourcePathAction = subResourcePath.Split("/").First();
        var subResourceNameOrSubAction = subResourcePath.Split("/").Last();

        switch (subResourcePathAction)
        {
            case "issue":
                if (resourcePathDescriptor.ResourceName != "root-ca" && subResourcePath == "issue/self-signed")
                {
                    throw new InvalidOperationException("Resource name must be 'ca' for self-signed certificate authority.");
                }

                if (resourcePathDescriptor.ResourceName == "root-ca" && subResourcePath != "issue/self-signed")
                {
                    throw new InvalidOperationException("You can only issue self-signed requests for the root-ca");
                }

                return await this.InitializeInternalCA(httpContext, resourcePathDescriptor, resourcePathDescriptor.ResourceName == "root-ca" ? null : subResourceNameOrSubAction, cancellationToken);

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

    private async Task<IResult> InitializeInternalCA(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, string? rootCaId = null, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        X509Certificate2? issuerCertificate = null;

        if (await configStore.GetAsync<SystemObject>(resourcePathDescriptor.ResourceId, cancellationToken) != null)
        {
            return Results.BadRequest();
        }

        if (!string.IsNullOrWhiteSpace(rootCaId))
        {
            var rootCa = await configStore.GetAsync<CertificateAuthority>(resourcePathDescriptor.GetResourceId(rootCaId), cancellationToken);

            if (rootCa == null)
            {
                return Results.BadRequest();
            }

            var pemData = new PemCertificateData()
            {
                EncodedCertificate = rootCa.Data.Certificate!,
                FriendlyName = rootCa.Data.FriendlyName!,
                EncodedPrivateKey = rootCa.Data.PrivateKey!
            };

            issuerCertificate = pemData.GetX509(true);
        }

        var friendlyName = "Kuiper Clustering Service";

        var certificate = CertificateGenerator.CreateCertificateAuthority(
            friendlyName,
            "CN=Kuiper CA,OU=Kuiper Clustering Service,O=Kuiper Microsystems,C=US",
            issuerCertificate == null ? 30 : 2, issuerCertificate);

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
                Certificate = certificate.EncodedCertificate,
                PrivateKey = certificate.EncodedPrivateKey,
            }
        };

        var result = await configStore.SetAsync(resourcePathDescriptor.ResourceId, certificateAuthority, cancellationToken);

        return Results.Created(resourcePathDescriptor.ResourceId, result);
    }
}
