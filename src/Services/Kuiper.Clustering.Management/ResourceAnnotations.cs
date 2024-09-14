namespace Kuiper.Clustering.ServiceApi
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ResourceTypeAttribute : Attribute
    {
        public string Group { get; }
        public string Version { get; }
        public string ResourceType { get; }

        public ResourceTypeAttribute(string group, string version, string resourceType)
        {
            Group = group;
            Version = version;
            ResourceType = resourceType;
        }
    }
}
