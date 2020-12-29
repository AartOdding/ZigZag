using System.Text.Json;


namespace ZigZag.Core
{
    public class GroupNode : Node
    {
        public sealed override void Update()
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

        internal new void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Stuff", "yes");

            writer.WriteEndObject();
        }
        internal new void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);
            Serialization.FinishCurrentObject(ref reader);
            Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);
        }

    }
}
