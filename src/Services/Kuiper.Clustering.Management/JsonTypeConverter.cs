using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi
{
    public class JsonTypeConverter : JsonConverter<Type>
    {
        public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // We don't support it ... it's a security issue ...
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.FullName);
        }
    }
}
