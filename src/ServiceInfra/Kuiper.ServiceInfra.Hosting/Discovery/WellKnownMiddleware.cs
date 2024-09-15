//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Kuiper.ServiceInfra.Hosting.Discovery;

public class WellKnownMiddleware : IMiddleware
{
    protected readonly KuiperEndpointConfiguration config;

    public WellKnownMiddleware(KuiperEndpointConfiguration config)
    {
        this.config = config;
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
