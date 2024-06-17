using Kuiper.Clustering.ServiceApi.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Kuiper.Clustering.ServiceApi.Serialization
{
    public class SystemObjectMetadataTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(SystemObjectMetadata);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var metadata = new SystemObjectMetadata
            {
                Annotations = new Dictionary<string, object>(),
                Properties = new Dictionary<string, object>()
            };

            parser.Consume<MappingStart>();

            while (parser.TryConsume<Scalar>(out var propertyName))
            {
                var propertyValue = this.ReadValue(parser);
                if (propertyName.Value == "annotations")
                {
                    metadata.Annotations = (Dictionary<string, object>)propertyValue;
                }
                else
                {
                    metadata.Properties[propertyName.Value] = propertyValue;
                }
            }

            parser.Consume<MappingEnd>();

            return metadata;
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var metadata = (SystemObjectMetadata)value;

            if (metadata == null)
            {
                return;
            }

            emitter.Emit(new MappingStart());

            if (metadata?.Annotations != null)
            {
                emitter.Emit(new Scalar("annotations"));
                this.WriteValue(emitter, metadata.Annotations);
            }

            if (metadata?.Properties != null)
            {
                foreach (var property in metadata.Properties)
                {
                    emitter.Emit(new Scalar(property.Key));
                    this.WriteValue(emitter, property.Value);
                }
            }

            emitter.Emit(new MappingEnd());
        }

        private object ReadValue(IParser parser)
        {
            parser.TryConsume<Scalar>(out var scalarValue);
            if (scalarValue != null)
            {
                return scalarValue.Value;
            }

            parser.TryConsume<SequenceStart>(out var sequenceStart);
            if (sequenceStart != null)
            {
                var list = new List<object>();
                while (!parser.Accept<SequenceEnd>())
                {
                    list.Add(this.ReadValue(parser));
                }
                parser.Consume<SequenceEnd>();
                return list;
            }

            parser.TryConsume<MappingStart>(out var mappingStart);
            if (mappingStart != null)
            {
                var dict = new Dictionary<string, object>();

                while (!parser.Accept<MappingEnd>(out var _))
                {
                    var key = this.ReadValue(parser).ToString();
                    var value = this.ReadValue(parser);
                    dict[key] = value;
                }

                return dict;
            }

            return null;
        }

        private void WriteValue(IEmitter emitter, object value)
        {
            if (value is IDictionary<string, object> dict)
            {
                emitter.Emit(new MappingStart());
                foreach (var kvp in dict)
                {
                    emitter.Emit(new Scalar(kvp.Key));
                    this.WriteValue(emitter, kvp.Value);
                }
                emitter.Emit(new MappingEnd());
            }
            else if (value is IEnumerable<object> list)
            {
                emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                foreach (var item in list)
                {
                    this.WriteValue(emitter, item);
                }
                emitter.Emit(new SequenceEnd());
            }
            else
            {
                emitter.Emit(new Scalar(value?.ToString()));
            }
        }
    }
}