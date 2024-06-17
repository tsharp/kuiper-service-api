using Kuiper.Clustering.ServiceApi;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public abstract class SingleNamepsaceResourceServiceBase : IResourceServiceHandler
    {
        protected const string KuiperSystemNamespace = "kuiper-system";

        protected readonly string Namespace;

        public SingleNamepsaceResourceServiceBase(string @namespace)
        {
            Namespace = @namespace.ToLowerInvariant();
        }


        public async Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (!string.Equals(resourcePathDescriptor.Namespace, Namespace, StringComparison.InvariantCultureIgnoreCase))
            {
                return Results.BadRequest(new
                {
                    error = new
                    {
                        message = $"This service only responds to the `{Namespace}` namespace."
                    }
                });
            }

            return await this.HandleInternalRequest(httpContext, resourcePathDescriptor, cancellationToken);
        }

        protected abstract Task<IResult> HandleInternalRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default);
    }
}
