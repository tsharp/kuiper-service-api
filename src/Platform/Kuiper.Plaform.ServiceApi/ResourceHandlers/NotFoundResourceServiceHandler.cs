//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kuiper.Plaform.ServiceApi.ResourceHandlers;

internal class NotFoundResourceServiceHandler : IResourceServiceHandler
{
    public Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Results.NotFound());
    }
}
