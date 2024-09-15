//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System;

namespace Kuiper.Plaform.ServiceApi.ResourceHandlers;

internal static class ResourcePathParser
{
    public static ResourcePathDescriptor Parse(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
        {
            throw new ArgumentException("Full path cannot be null or empty.", nameof(fullPath));
        }

        var segments = fullPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length < 4)
        {
            throw new ArgumentException("Full path must have at least 4 segments: group, version, namespace, and resourceType.");
        }

        return new ResourcePathDescriptor(
            segments[0],
            segments[1],
            segments[2],
            segments[3],
            segments.Length > 4 ? segments[4] : null,
            segments.Length > 5 ? string.Join('/', segments, 5, segments.Length - 5) : null
        );
    }
}
