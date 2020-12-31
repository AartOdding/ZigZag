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
            if (node is not null)
            {
                Node = node;
                node.AddNodeParameter(this);
            }
        }

        public abstract void Update();
        public abstract bool Accepts(NodeParameter parameter);

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

        public bool Changed
        {
            get;
            protected set;
        }

        public NodeParameter Listened
        {
            get
            {
                if (m_listened is null)
                {
                    return null;
                }
                else
                {
                    return m_listened.Object;
                }
            }
            internal set
            {
                if (value is null)
                {
                    m_listened = null;
                }
                else
                {
                    m_listened = new ObjectRef<NodeParameter>(value);
                }
            }
        }

        public IEnumerable<NodeParameter> Listeners
        {
            get
            {
                if (m_listeners is null)
                {
                    return Enumerable.Empty<NodeParameter>();
                }
                else
                {
                    return m_listeners.Select(objRef => objRef.Object);
                }
            }
        }

        public int ListenerCount
        {
            get
            {
                if (m_listeners is null)
                {
                    return 0;
                }
                else
                {
                    return m_listeners.Count;
                }
            }
        }

        public bool IsListening()
        {
            if (m_listened is null)
            {
                return false;
            }
            else
            {
                return m_listened.Object is not null;
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(Listened));
            JsonSerializer.Serialize(writer, m_listened, options);

            writer.WritePropertyName(nameof(Listeners));
            JsonSerializer.Serialize(writer, m_listeners, options);
            
            writer.WriteEndObject();
        }

        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
            
            SerializationUtils.MustReadPropertyName(ref reader, nameof(Listened));
            m_listened = JsonSerializer.Deserialize<ObjectRef<NodeParameter>>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, nameof(Listeners));
            m_listeners = JsonSerializer.Deserialize<List<ObjectRef<NodeParameter>>>(ref reader, options);

            SerializationUtils.FinishCurrentObject(ref reader);
        }

        internal void SetChanged(bool changed)
        {
            Changed = changed;
        }

        internal void AddListener(NodeParameter listener)
        {
            Debug.Assert(listener is not null);
            Debug.Assert(!ContainsListener(listener));

            if (m_listeners is null)
            {
                m_listeners = new List<ObjectRef<NodeParameter>>();
            }
            m_listeners.Add(new ObjectRef<NodeParameter>(listener));
        }
        internal void RemoveListener(NodeParameter listener)
        {
            Debug.Assert(ContainsListener(listener));

            for (int i = 0; i < m_listeners.Count; ++i)
            {
                if (m_listeners[i].Object == listener)
                {
                    m_listeners.RemoveAt(i);
                    break;
                }
            }
            if (m_listeners.Count == 0)
            {
                m_listeners = null;
            }
        }
        internal bool ContainsListener(NodeParameter listener)
        {
            if (m_listeners is null)
            {
                return false;
            }
            else
            {
                foreach (var l in m_listeners)
                {
                    if (l.Object == listener)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private ObjectRef<NodeParameter> m_listened;
        private List<ObjectRef<NodeParameter>> m_listeners;
    }
}
