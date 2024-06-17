namespace Kuiper.Clustering.ServiceApi
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ResourceTypeAttribute : Attribute
    {
        public string Api { get; }
        public string Version { get; }
        public string ResourceType { get; }

        public ResourceTypeAttribute(string api, string version, string resourceType)
        {
            Api = api;
            Version = version;
            ResourceType = resourceType;
        }
    }
}
