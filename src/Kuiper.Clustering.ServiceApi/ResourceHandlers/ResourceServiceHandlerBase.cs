using Json.Patch;
using Kuiper.Clustering.ServiceApi.Dto;
using Kuiper.Clustering.ServiceApi.Storage;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public abstract class ResourceServiceHandlerBase<TSystemObject> : IResourceServiceHandler
        where TSystemObject : SystemObject
    {
        protected readonly IKeyValueStore configStore;

        protected abstract string ResourceKindName { get; }

        public delegate Task<IResult> RequestHandler(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default);

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

            if (!string.IsNullOrWhiteSpace(resourcePathDescriptor.SubResourcePath) && httpContext.Request.Method != "POST")
            {
                return Task.FromResult(Results.BadRequest("Subresource path is not supported"));
            }

            switch (httpContext.Request.Method)
            {
                case "GET":
                    return this.HandleGetRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "PUT":
                    return this.HandlePutRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "DELETE":
                    return this.HandleDeleteRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "PATCH":
                    return this.HandlePatchRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "POST":
                    return this.HandlePostRequest(httpContext, resourcePathDescriptor, cancellationToken);
                default:
                    return this.MethodNotAllowed();
            }
        }

        protected Task<IResult> MethodNotAllowed()
        {
            return Task.FromResult(Results.Problem("Method not allowed", statusCode: StatusCodes.Status405MethodNotAllowed));
        }

        protected virtual Task<IResult> HandlePostRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            return this.MethodNotAllowed();
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

            var existing = await configStore.GetAsync<TSystemObject>(resourcePathDescriptor.ResourceId);

            try
            {
                var systemObject = await httpContext.Request.ReadFromJsonAsync<TSystemObject>();

                if (existing == null)
                {
                    systemObject = await configStore.SetAsync(resourcePathDescriptor.ResourceId, systemObject);
                    return Results.Created(resourcePathDescriptor.ResourceId, systemObject);
                }

                systemObject = await configStore.SetAsync(resourcePathDescriptor.ResourceId, systemObject);
                return Results.Ok(systemObject);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        protected virtual async Task<IResult> HandlePatchRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            try
            {
                var patchSet = await httpContext.Request.ReadFromJsonAsync<JsonPatch>(cancellationToken);

                if (patchSet == null || !patchSet.Operations.Any())
                {
                    return Results.NoContent();
                }

                var existingData = await configStore.GetAsync<TSystemObject>(resourcePathDescriptor.ResourceId);

                if (existingData == null)
                {
                    return Results.NotFound();
                }

                var patchedData = patchSet.Apply(existingData);

                await configStore.SetAsync(resourcePathDescriptor.ResourceId, patchedData);

                return Results.Ok(patchedData);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        protected virtual async Task<IResult> HandleGetRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                var config = await configStore.GetAsync<TSystemObject>(resourcePathDescriptor.ResourceId);

                if (config == null)
                {
                    return Results.NotFound();
                }

                return Results.Json(config);
            }


            return Results.Json(await configStore.ScanAsync<TSystemObject>(resourcePathDescriptor.ResourceTypeId, cancellationToken));
        }
    }
}
