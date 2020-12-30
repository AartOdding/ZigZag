using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core.Parameters
{
    public abstract class NodeParameter : ZObject
    {
        public NodeParameter()
        {
        }

        public NodeParameter(Node node)
        {
            Node = node;
            if (!(node is null))
            {
                node.AddNodeParameter(this);
            }
        }

        public string Name
        {
            get;
            internal set;
        }

        public Node Node
        {
            get;
            internal set;
        }

        public abstract void Update();

        public abstract bool Accepts(NodeParameter parameter);

        public bool Changed
        {
            get;
            protected set;
        }

        public bool IsListening()
        {
            return !(ListenedParameter is null);
        }

        internal void SetChanged(bool changed)
        {
            Changed = changed;
        }

        public NodeParameter ListenedParameter
        {
            get
            {
                return m_listenedParameter.Object;
            }
        }

        public IEnumerable<NodeParameter> ListeningParameters
        {
            get
            {
                return m_listeningParameters.Select(objRef => objRef.Object);
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("ListenedParameter");
            JsonSerializer.Serialize(writer, m_listenedParameter, options);

            writer.WritePropertyName("ListeningParameters");
            JsonSerializer.Serialize(writer, m_listeningParameters, options);
            
            writer.WriteEndObject();
        }

        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
            
            SerializationUtils.MustReadPropertyName(ref reader, "ListenedParameter");
            m_listenedParameter = JsonSerializer.Deserialize<ObjectRef<NodeParameter>>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, "ListeningParameters");
            m_listeningParameters = JsonSerializer.Deserialize<List<ObjectRef<NodeParameter>>>(ref reader, options);

            SerializationUtils.FinishCurrentObject(ref reader);
        }

        internal ObjectRef<NodeParameter> m_listenedParameter = new ObjectRef<NodeParameter>();
        internal List<ObjectRef<NodeParameter>> m_listeningParameters = new List<ObjectRef<NodeParameter>>();
    }
}
