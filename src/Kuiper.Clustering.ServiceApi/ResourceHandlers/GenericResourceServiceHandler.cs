using Kuiper.Clustering.ServiceApi.Storage;

namespace Kuiper.Clustering.ServiceApi.ResourceHandlers
{
    public class GenericResourceServiceHandler : IResourceServiceHandler
    {
        private readonly IKeyValueStore configStore;

        public GenericResourceServiceHandler(IKeyValueStore configStore)
        {
            this.configStore = configStore;
        }

        public Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            switch (httpContext.Request.Method)
            {
                case "GET":
                    return this.HandleGetRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "PUT":
                    return this.HandlePutRequest(httpContext, resourcePathDescriptor, cancellationToken);
                case "DELETE":
                    return this.HandleDeleteRequest(httpContext, resourcePathDescriptor, cancellationToken);
                default:
                    return Task.FromResult(Results.Problem("Method not allowed", statusCode: StatusCodes.Status405MethodNotAllowed));
            }
        }

        private async Task<IResult> HandleDeleteRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
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

        private async Task<IResult> HandlePutRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                return Results.BadRequest("Resource name is required");
            }

            IDictionary<string, object>? body = null;

            try
            {
                body = await httpContext.Request.ReadFromJsonAsync<IDictionary<string, object>>();
            }
            catch
            {
            }

            if (body == null)
            {
                return Results.BadRequest();
            }

            body.Add("apiVersion", resourcePathDescriptor.ApiVersion);
            body.Add("namespace", resourcePathDescriptor.Namespace);
            body.Add("name", resourcePathDescriptor.ResourceName);

            await configStore.SetAsync(resourcePathDescriptor.ResourceId, body);

            return Results.Created(resourcePathDescriptor.ResourceId, body);
        }

        private async Task<IResult> HandleGetRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(resourcePathDescriptor.ResourceName))
            {
                var config = await configStore.GetAsync<object>(resourcePathDescriptor.ResourceId);

                if (config == null)
                {
                    return Results.NotFound();
                }

                return Results.Json(config);
            }

            
            return Results.Json(await configStore.ScanAsync<object>(resourcePathDescriptor.ResourceTypeId, cancellationToken));
        }        
    }
}
