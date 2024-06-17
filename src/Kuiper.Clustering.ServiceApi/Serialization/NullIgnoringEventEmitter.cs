using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace Kuiper.Clustering.ServiceApi.Serialization
{
    public class NullValueSkippingEventEmitter : ChainedEventEmitter
    {
        public NullValueSkippingEventEmitter(IEventEmitter nextEmitter) : base(nextEmitter)
        {
        }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                // Skip emitting null values
                return;
            }

            base.Emit(eventInfo, emitter);
        }

        public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                // Skip emitting null values
                return;
            }
            base.Emit(eventInfo, emitter);
        }

        public override void Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                // Skip emitting null values
                return;
            }
            base.Emit(eventInfo, emitter);
        }

        public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                // Skip emitting null values
                return;
            }
            base.Emit(eventInfo, emitter);
        }

        public override void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                // Skip emitting null values
                return;
            }
            base.Emit(eventInfo, emitter);
        }
    }
}
