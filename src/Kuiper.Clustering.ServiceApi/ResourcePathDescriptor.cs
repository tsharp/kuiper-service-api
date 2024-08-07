﻿using Kuiper.Clustering.ServiceApi.Dto;
using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi
{
    public class ResourcePathDescriptor
    {
        public ResourcePathDescriptor()
        {
        }

        public ResourcePathDescriptor(SystemObject @object) : 
            this(   @object.ApiVersion,
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
            Group = group.ToLowerInvariant();
            Version = version.ToLowerInvariant();
            Namespace = @namespace.ToLowerInvariant();
            ResourceKind = resourceKind.ToLowerInvariant();
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

    public static class ResourcePathParser
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
}
