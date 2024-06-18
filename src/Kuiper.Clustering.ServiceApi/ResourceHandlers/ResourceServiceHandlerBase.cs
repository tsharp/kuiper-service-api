using Kuiper.Clustering.ServiceApi.Models;
using Kuiper.Clustering.ServiceApi.Storage;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public abstract class ResourceServiceHandlerBase : IResourceServiceHandler
    {
        protected readonly IKeyValueStore configStore;

        public ResourceServiceHandlerBase(IKeyValueStore configStore)
        {
            this.configStore = configStore;
        }

        protected virtual bool CanHandleResourceRequest(ResourcePathDescriptor resourcePathDescriptor, out IResult? result)
        {
            result = null;

            return true;
        }

        public Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (!this.CanHandleResourceRequest(resourcePathDescriptor, out IResult? result))
            {
                return Task.FromResult(result ?? Results.BadRequest());
            }

            switch (httpContext.Request.Method)
            {
                case "GET":
                    return this.HandleGetRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "PUT":
                    return this.HandlePutRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "DELETE":
                    return this.HandleDeleteRequest(httpContext, resourcePathDescriptor, cancellationToken);
                default:
                    return this.MethodNotAllowed();
            }
        }

        protected Task<IResult> MethodNotAllowed()
        {
            return Task.FromResult(Results.Problem("Method not allowed", statusCode: StatusCodes.Status405MethodNotAllowed));
        }

        protected virtual async Task<IResult> HandleDeleteRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                return Results.BadRequest("Resource name is required");
            }

            if (await configStore.RemoveAsync(resourcePathDescriptor.ResourceId))
            {
                return Results.NoContent();
            }

            return Results.NotFound();
        }

        protected virtual async Task<IResult> HandlePutRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                return Results.BadRequest("Resource name is required");
            }

            SystemObject? body = null;

            try
            {
                body = await httpContext.Request.ReadFromJsonAsync<SystemObject>();
            }
            catch
            {
            }

            if (body == null)
            {
                return Results.BadRequest();
            }

            body.ApiVersion = resourcePathDescriptor.ApiVersion;
            body.Metadata = body.Metadata ?? new SystemObjectMetadata();
            body.Metadata.Name = resourcePathDescriptor.ResourceName;

            await configStore.SetAsync(resourcePathDescriptor.ResourceId, body);

            return Results.Created(resourcePathDescriptor.ResourceId, body);
        }

        protected virtual async Task<IResult> HandleGetRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                var config = await configStore.GetAsync<SystemObject>(resourcePathDescriptor.ResourceId);

                if (config == null)
                {
                    return Results.NotFound();
                }

                return Results.Json(config);
            }


            return Results.Json(await configStore.ScanAsync<SystemObject>(resourcePathDescriptor.ResourceTypeId, cancellationToken));
        }
    }
}
