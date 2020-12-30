using System.Collections.Generic;
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

            writer.WriteStartArray("NodeInputs");
            foreach (var nodeInput in NodeInputs)
            {
                JsonSerializer.Serialize(writer, nodeInput, nodeInput.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("NodeOutputs");
            foreach (var nodeOutput in NodeOutputs)
            {
                JsonSerializer.Serialize(writer, nodeOutput, nodeOutput.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("NodeParameters");
            foreach (var nodeParameter in NodeParameters)
            {
                JsonSerializer.Serialize(writer, nodeParameter, nodeParameter.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("ChildNodes");
            foreach (var childNode in ChildNodes)
            {
                JsonSerializer.Serialize(writer, childNode, childNode.GetType(), options);
            }
            writer.WriteEndArray();

            // When adding more properties make sure to add them here.

            writer.WriteEndObject();
        }
        public void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);

            SerializationUtils.MustReadPropertyName(ref reader, "Name");
            reader.Read();
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Null);
            if (reader.TokenType == JsonTokenType.String)
            {
                Name = reader.GetString();
            }

            SerializationUtils.MustReadPropertyName(ref reader, "NodeInputs");
            SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.StartArray);
            reader.Read();
            while(reader.TokenType != JsonTokenType.EndArray)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
                var nodeInput = JsonSerializer.Deserialize<NodeInput>(ref reader, options);
                nodeInput.Node = this;
                AddNodeInput(nodeInput);
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }

            SerializationUtils.MustReadPropertyName(ref reader, "NodeOutputs");
            SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.StartArray);
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
                var nodeOutput = JsonSerializer.Deserialize<NodeOutput>(ref reader, options);
                nodeOutput.Node = this;
                AddNodeOutput(nodeOutput);
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }

            SerializationUtils.MustReadPropertyName(ref reader, "NodeParameters");
            SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.StartArray);
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
                var nodeParameter = JsonSerializer.Deserialize<NodeParameter>(ref reader, options);
                nodeParameter.Node = this;
                AddNodeParameter(nodeParameter);
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }

            SerializationUtils.MustReadPropertyName(ref reader, "ChildNodes");
            SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.StartArray);
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);
                var node = JsonSerializer.Deserialize<Node>(ref reader, options);
                node.ParentNode = this;
                AddChildNode(node);
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }

            SerializationUtils.FinishCurrentObject(ref reader);
            SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
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
            if (m_childNodes is null)
            {
                m_childNodes = new List<Node>();
            }
            m_childNodes.Add(childNode);
        }
        internal void AddNodeInput(NodeInput nodeInput)
        {
            if (m_nodeInputs is null)
            {
                m_nodeInputs = new List<NodeInput>();
            }
            m_nodeInputs.Add(nodeInput);
        }
        internal void AddNodeOutput(NodeOutput nodeOutput)
        {
            if (m_nodeOutputs is null)
            {
                m_nodeOutputs = new List<NodeOutput>();
            }
            m_nodeOutputs.Add(nodeOutput);
        }
        internal void AddNodeParameter(NodeParameter nodeParameter)
        {
            if (m_nodeParameters is null)
            {
                m_nodeParameters = new List<NodeParameter>();
            }
            m_nodeParameters.Add(nodeParameter);
        }
        internal void RemoveChildNode(Node childNode)
        {
            m_childNodes.Remove(childNode);
        }
        internal void RemoveNodeInput(NodeInput nodeInput)
        {
            m_nodeInputs.Remove(nodeInput);
        }
        internal void RemoveNodeOutput(NodeOutput nodeOutput)
        {
            m_nodeOutputs.Remove(nodeOutput);
        }
        internal void RemoveNodeParameter(NodeParameter nodeParameter)
        {
            m_nodeParameters.Remove(nodeParameter);
        }

        private List<Node> m_childNodes;
        private List<NodeInput> m_nodeInputs;
        private List<NodeOutput> m_nodeOutputs;
        private List<NodeParameter> m_nodeParameters;
    }
}
