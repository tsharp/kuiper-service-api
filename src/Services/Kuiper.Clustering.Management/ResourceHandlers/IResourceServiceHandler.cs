using Kuiper.Clustering.Management;

namespace Kuiper.Clustering.Management.ResourceHandlers
{
    public interface IResourceServiceHandler
    {
        Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default);
    }
}
