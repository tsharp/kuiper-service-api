using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi
{
    public class ResourcePathDescriptor
    {
        public ResourcePathDescriptor()
        {
        }

        public ResourcePathDescriptor(
            string api,
            string version,
            string @namespace,
            string resourceType,
            string? resourceName,
            string? subResourcePath)
        {
            Api = api.ToLowerInvariant();
            Version = version.ToLowerInvariant();
            Namespace = @namespace.ToLowerInvariant();
            ResourceType = resourceType.ToLowerInvariant();
            ResourceName = resourceName?.ToLowerInvariant();
            SubResourcePath = subResourcePath?.ToLowerInvariant();
        }

        public string Api { get; set; }
        public string Version { get; set; }

        public string Namespace { get; set; }

        public string ResourceType { get; set; }

        public string? ResourceName { get; set; }

        public string? SubResourcePath { get; set; }

        [JsonConverter(typeof(JsonTypeConverter))]
        public Type HandlerType { get; set; }

        public string ApiVersion
        {
            get => $"{Api}/{Version}";
        }

        public string ResourceTypeId
        {
            get => $"/{ApiVersion}/{Namespace}/{ResourceType}";
        }

        public string ResourceId
        {
            get => $"{ResourceTypeId}/{ResourceName}";
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
                throw new ArgumentException("Full path must have at least 4 segments: api, version, namespace, and resourceType.");
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
