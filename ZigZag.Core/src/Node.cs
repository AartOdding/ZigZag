using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using ZigZag.Core.Parameters;
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
                parent.AddChildNode(this);
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
        public IEnumerable<Node> ChildNodes
        {
            get
            {
                if (m_childNodes is null)
                {
                    return Enumerable.Empty<Node>();
                }
                else
                {
                    return m_childNodes;
                }
            }
        }
        public int ChildNodeCount
        {
            get
            {
                if (m_childNodes is null)
                {
                    return 0;
                }
                else
                {
                    return m_childNodes.Count;
                }
            }
        }
        public IEnumerable<NodeInput> NodeInputs
        {
            get
            {
                if (m_nodeInputs is null)
                {
                    return Enumerable.Empty<NodeInput>();
                }
                else
                {
                    return m_nodeInputs;
                }
            }
        }
        public int NodeInputCount
        {
            get
            {
                if (m_nodeInputs is null)
                {
                    return 0;
                }
                else
                {
                    return m_nodeInputs.Count;
                }
            }
        }
        public IEnumerable<NodeOutput> NodeOutputs
        {
            get
            {
                if (m_nodeOutputs is null)
                {
                    return Enumerable.Empty<NodeOutput>();
                }
                else
                {
                    return m_nodeOutputs;
                }
            }
        }
        public int NodeOutputCount
        {
            get
            {
                if (m_nodeOutputs is null)
                {
                    return 0;
                }
                else
                {
                    return m_nodeOutputs.Count;
                }
            }
        }
        public IEnumerable<NodeParameter> NodeParameters
        {
            get
            {
                if (m_nodeParameters is null)
                {
                    return Enumerable.Empty<NodeParameter>();
                }
                else
                {
                    return m_nodeParameters;
                }
            }
        }
        public int NodeParameterCount
        {
            get
            {
                if (m_nodeParameters is null)
                {
                    return 0;
                }
                else
                {
                    return m_nodeParameters.Count;
                }
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

        internal void AddChildNode(Node childNode)
        {
            Debug.Assert(!(childNode is null));
            if (m_childNodes is null)
            {
                m_childNodes = new List<Node>();
            }
            m_childNodes.Add(childNode);
        }
        internal void AddNodeInput(NodeInput nodeInput)
        {
            Debug.Assert(!(nodeInput is null));
            if (m_nodeInputs is null)
            {
                m_nodeInputs = new List<NodeInput>();
            }
            m_nodeInputs.Add(nodeInput);
        }
        internal void AddNodeOutput(NodeOutput nodeOutput)
        {
            Debug.Assert(!(nodeOutput is null));
            if (m_nodeOutputs is null)
            {
                m_nodeOutputs = new List<NodeOutput>();
            }
            m_nodeOutputs.Add(nodeOutput);
        }
        internal void AddNodeParameter(NodeParameter nodeParameter)
        {
            Debug.Assert(!(nodeParameter is null));
            if (m_nodeParameters is null)
            {
                m_nodeParameters = new List<NodeParameter>();
            }
            m_nodeParameters.Add(nodeParameter);
        }
        internal void RemoveChildNode(Node childNode)
        {
            m_childNodes.Remove(childNode);
            if (m_childNodes.Count == 0)
            {
                m_childNodes = null;
            }
        }
        internal void RemoveNodeInput(NodeInput nodeInput)
        {
            m_nodeInputs.Remove(nodeInput);
            if (m_nodeInputs.Count == 0)
            {
                m_nodeInputs = null;
            }
        }
        internal void RemoveNodeOutput(NodeOutput nodeOutput)
        {
            m_nodeOutputs.Remove(nodeOutput);
            if (m_nodeOutputs.Count == 0)
            {
                m_nodeOutputs = null;
            }
        }
        internal void RemoveNodeParameter(NodeParameter nodeParameter)
        {
            m_nodeParameters.Remove(nodeParameter);
            if (m_nodeParameters.Count == 0)
            {
                m_nodeParameters = null;
            }
        }

        private List<Node> m_childNodes;
        private List<NodeInput> m_nodeInputs;
        private List<NodeOutput> m_nodeOutputs;
        private List<NodeParameter> m_nodeParameters;
    }
}
