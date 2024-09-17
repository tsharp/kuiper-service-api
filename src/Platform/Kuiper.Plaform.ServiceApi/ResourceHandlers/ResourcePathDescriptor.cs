//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Kuiper.Plaform.ManagementObjects;
using Kuiper.Plaform.ServiceApi.Data;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Kuiper.Plaform.ServiceApi.ResourceHandlers;

public sealed class ResourcePathDescriptor
{
    public ResourcePathDescriptor()
    {
    }

    public ResourcePathDescriptor(SystemObject @object) :
        this(
            @object.ApiVersion.Split('/').First(),
            @object.ApiVersion.Split('/').Last(),
            @object.Metadata.Namespace,
            @object.Kind,
            @object.Metadata.Name)
    {
    }

    public ResourcePathDescriptor(
        string group,
        string version,
        string @namespace,
        string resourceKind,
        string? resourceName = null,
        string? subResourcePath = null)
    {
        if (string.IsNullOrWhiteSpace(group))
        {
            throw new ArgumentNullException(nameof(group));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentNullException(nameof(version));
        }

        if (string.IsNullOrWhiteSpace(@namespace))
        {
            throw new ArgumentNullException(nameof(@namespace));
        }

        Group = group.ToLowerInvariant();
        Version = version.ToLowerInvariant();
        Namespace = @namespace.ToLowerInvariant();
        ResourceKind = resourceKind?.ToLowerInvariant();
        ResourceName = resourceName?.ToLowerInvariant();
        SubResourcePath = subResourcePath?.ToLowerInvariant();
    }

    public string Group { get; set; }

    public string Version { get; set; }

    public string Namespace { get; set; }

    public string ResourceKind { get; set; }

    public string? ResourceName { get; set; }

    public string? SubResourcePath { get; set; }

    [JsonConverter(typeof(JsonTypeConverter))]
    public Type HandlerType { get; set; }

    public string ApiVersion
    {
        get => $"{Group}/{Version}";
    }

    public string ResourceTypeId
    {
        get => $"/{ApiVersion}/{Namespace}/{ResourceKind}";
    }

    public string ResourceId
    {
        get => this.GetResourceId(ResourceName);
    }

    public string GetResourceId(string? resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentNullException(nameof(resourceName));
        }

        return $"{ResourceTypeId}/{resourceName}";
    }
}
