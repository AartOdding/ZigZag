using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core
{
    public abstract class NodeOutput : ZObject
    {
        public NodeOutput()
        {
        }
        public NodeOutput(Node node)
        {
            if (node is not null)
            {
                Node = node;
                node.m_nodeOutputs.Add(this);
            }
        }

        public abstract void Update();

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

        public IEnumerable<NodeInput> ConnectedInputs
        {
            get
            {
                if (m_connectedInputs is null)
                {
                    return Enumerable.Empty<NodeInput>();
                }
                else
                {
                    return m_connectedInputs.Select(objRef => objRef.Object); ;
                }
            }
        }

        public int ConnectedInputCount
        {
            get
            {
                if (m_connectedInputs is null)
                {
                    return 0;
                }
                else
                {
                    return m_connectedInputs.Count;
                }
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(ConnectedInputs));
            JsonSerializer.Serialize(writer, m_connectedInputs, options);

            writer.WriteEndObject();
        }

        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);

            SerializationUtils.MustReadPropertyName(ref reader, nameof(ConnectedInputs));
            m_connectedInputs = JsonSerializer.Deserialize<List<ObjectRef<NodeInput>>>(ref reader, options);

            SerializationUtils.FinishCurrentObject(ref reader);
        }

        internal void SetChanged(bool changed)
        {
            Changed = changed;
        }

        internal void AddConnectedInput(NodeInput input)
        {
            Debug.Assert(input is not null);
            Debug.Assert(!ContainsConnectedInput(input));

            if (m_connectedInputs is null)
            {
                m_connectedInputs = new List<ObjectRef<NodeInput>>();
            }
            m_connectedInputs.Add(new ObjectRef<NodeInput>(input));
        }
        internal void RemoveConnectedInput(NodeInput input)
        {
            Debug.Assert(ContainsConnectedInput(input));

            for (int i = 0; i < m_connectedInputs.Count; ++i)
            {
                if (m_connectedInputs[i].Object == input)
                {
                    m_connectedInputs.RemoveAt(i);
                    break;
                }
            }
            if (m_connectedInputs.Count == 0)
            {
                m_connectedInputs = null;
            }
        }
        internal bool ContainsConnectedInput(NodeInput input)
        {
            if (m_connectedInputs is null)
            {
                return false;
            }
            else
            {
                foreach (var i in m_connectedInputs)
                {
                    if (i.Object == input)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private List<ObjectRef<NodeInput>> m_connectedInputs;
    }
}
