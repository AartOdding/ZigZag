using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core
{
    public abstract class Node : ZObject
    {
        public Node()
        {
        }
        public Node(Node parent)
        {
            // No need to check for loops, because this node is just constructed it cannot
            // have any children so there can not be any loops.

            if (!(parent is null))
            {
                ParentNode = parent;
                parent.m_childNodes.Add(this);
            }
        }

        public string Name
        {
            get;
            internal set;
        }
        public Node ParentNode
        {
            get;
            internal set;
        }
        public IReadOnlyList<Node> ChildNodes
        {
            get
            {
                return m_childNodes;
            }
        }
        public IReadOnlyList<NodeInput> NodeInputs
        {
            get
            {
                return m_nodeInputs;
            }
        }
        public IReadOnlyList<NodeOutput> NodeOutputs
        {
            get
            {
                return m_nodeOutputs;
            }
        }
        public IReadOnlyList<NodeParameter> NodeParameters
        {
            get
            {
                return m_nodeParameters;
            }
        }

        public abstract void Update();

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Name", Name);

            writer.WritePropertyName("NodeInputs");
            JsonSerializer.Serialize(writer, m_nodeInputs, options);

            writer.WritePropertyName("NodeOutputs");
            JsonSerializer.Serialize(writer, m_nodeOutputs, options);

            writer.WritePropertyName("NodeParameters");
            JsonSerializer.Serialize(writer, m_nodeParameters, options);

            writer.WritePropertyName("ChildNodes");
            JsonSerializer.Serialize(writer, m_childNodes, options);

            writer.WriteEndObject();
        }
        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);

            SerializationUtils.MustReadPropertyName(ref reader, "Name");
            Name = JsonSerializer.Deserialize<string>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, "NodeInputs");
            m_nodeInputs = JsonSerializer.Deserialize<List<NodeInput>>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, "NodeOutputs");
            m_nodeOutputs = JsonSerializer.Deserialize<List<NodeOutput>>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, "NodeParameters");
            m_nodeParameters = JsonSerializer.Deserialize<List<NodeParameter>>(ref reader, options);

            SerializationUtils.MustReadPropertyName(ref reader, "ChildNodes");
            m_childNodes = JsonSerializer.Deserialize<List<Node>>(ref reader, options);

            SerializationUtils.FinishCurrentObject(ref reader);
        }

        public bool IsParentOf(Node child)
        {
            if (!(child is null))
            {
                return child.IsChildOf(this);
            }
            return false;
        }
        public bool IsChildOf(Node parent)
        {
            return ParentNode == parent;
        }
        public bool IsIndirectParentOf(Node child)
        {
            if (!(child is null))
            {
                return child.IsIndirectChildOf(this);
            }
            return false;
        }
        public bool IsIndirectChildOf(Node parent)
        {
            Node p = ParentNode;

            while (!(p is null))
            {
                if (p == parent)
                {
                    return true;
                }
                p = p.ParentNode;
            }
            return p == parent;
        }

        public IEnumerable<T> GetChildren<T>() where T : Node
        {
            if (m_childNodes is null)
            {
                yield break;
            }
            foreach (var child in m_childNodes)
            {
                if (child is T c)
                {
                    yield return c;
                }
            }
        }

        internal List<Node> m_childNodes = new List<Node>();
        internal List<NodeInput> m_nodeInputs = new List<NodeInput>();
        internal List<NodeOutput> m_nodeOutputs = new List<NodeOutput>();
        internal List<NodeParameter> m_nodeParameters = new List<NodeParameter>();
    }
}
