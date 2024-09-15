//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kuiper.Plaform.ServiceApi.ResourceHandlers;

public interface IResourceServiceHandler
{
    Task<IResult> HandleRequest(HttpContext httpContext, ResourcePathDescriptor resourcePathDescriptor, CancellationToken cancellationToken = default);
}
