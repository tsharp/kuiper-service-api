namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public class NotFoundResourceServiceHandler : IResourceServiceHandler
    {
        public Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Results.NotFound());
        }
    }
}
