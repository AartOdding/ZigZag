using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core
{
    public abstract class NodeInput : ZObject
    {
        public NodeInput()
        {
        }

        public NodeInput(Node node)
        {
            if (node is not null)
            {
                Node = node;
                node.AddNodeInput(this);
            }
        }

        public abstract void Update();
        public abstract bool Accepts(NodeOutput output);

        public Node Node
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            internal set;
        }

        public NodeOutput ConnectedOutput
        {
            get
            {
                if (m_connectedOutput is null)
                {
                    return null;
                }
                else
                {
                    return m_connectedOutput.Object;
                }
            }
            internal set
            {
                if (value is null)
                {
                    m_connectedOutput = null;
                }
                else
                {
                    m_connectedOutput = new ObjectRef<NodeOutput>(value);
                }
            }
        }

        public bool IsConnected()
        {
            if (m_connectedOutput is null)
            {
                return false;
            }
            else
            {
                return m_connectedOutput.Object is not null;
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(ConnectedOutput));
            JsonSerializer.Serialize(writer, m_connectedOutput, options);

            writer.WriteEndObject();
        }

        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);

            SerializationUtils.MustReadPropertyName(ref reader, nameof(ConnectedOutput));
            m_connectedOutput = JsonSerializer.Deserialize<ObjectRef<NodeOutput>>(ref reader, options);

            SerializationUtils.FinishCurrentObject(ref reader);
        }

        private ObjectRef<NodeOutput> m_connectedOutput;
    }
}
