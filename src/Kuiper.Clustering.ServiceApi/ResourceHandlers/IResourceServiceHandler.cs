namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public interface IResourceServiceHandler
    {
        Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default);
    }
}
