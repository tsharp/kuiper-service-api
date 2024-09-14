using Kuiper.Clustering.Management.Middware;
using Kuiper.Clustering.Management.Storage;
using Kuiper.Clustering.ServiceApi.Resources;

namespace Kuiper.Clustering.Management.Middware.Discovery
{
    public class WellKnownMiddleware : IMiddleware
    {
        protected readonly IKeyValueStore configStore;
        protected readonly KuiperEndpointConfiguration config;

        public WellKnownMiddleware(IKeyValueStore configStore, KuiperEndpointConfiguration config)
        {
            this.config = config;
            this.configStore = configStore;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var response = new WellKnownEndpointResponse()
            {
                WellKnownEndpoint = $"{context.GetRequestBaseUri()}/{config.WellKnownEndpoint}",
                KeysEndpoint = $"{context.GetRequestBaseUri()}/{config.KeysEndpoint}",
                CaEndpoint = $"{context.GetRequestBaseUri()}/{config.CaEndpoint}"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

}
