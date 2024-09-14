using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kuiper.Clustering.ServiceApi.Serialization
{
    internal static class YamlExtensions
    {
        public static string SerializeToYaml(this object value)
        {
            var serializer = new SerializerBuilder()
                .WithTypeConverter(new JsonElementYamlTypeConverter())
                .WithTypeInspector(x => new SortedTypeInspector(x))
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            return serializer.Serialize(value);
        }
    }
}
