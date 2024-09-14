using Kuiper.Clustering.Management;

namespace Kuiper.Clustering.Management.ResourceHandlers
{
    public class NotFoundResourceServiceHandler : IResourceServiceHandler
    {
        public Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Results.NotFound());
        }
    }
}
