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

        internal override void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Name", Name);

            writer.WriteStartArray("NodePorts");

            foreach (var port in m_nodePorts)
            {
                JsonSerializer.Serialize(writer, port, port.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("ChildNodes");

            foreach (var child in m_childNodes)
            {
                JsonSerializer.Serialize(writer, child, child.GetType(), options);
            }

            writer.WriteEndArray();

            // When adding more properties make sure to add them here.

            writer.WriteEndObject();
        }
        internal override void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {

        }

    }
}
