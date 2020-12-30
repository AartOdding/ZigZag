using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core
{
    public class GroupNode : Node
    {
        public override void Update()
        {

        }

        public virtual void EarlyUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public AbstractExecutor Executor
        {
            get
            {
                return m_executor;
            }

            set
            {
                m_executor = value;

                if (!(m_executor is null))
                {
                    m_executor.Node = this;
                }
            }
        }

        private AbstractExecutor m_executor;

        public new void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Stuff", "yes");

            writer.WriteEndObject();
        }
        public new void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
            SerializationUtils.FinishCurrentObject(ref reader);
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
        }

    }
}
