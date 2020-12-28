using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ZigZag.Core.Parameters;


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
        public Node(string name)
        {
            Name = name;
        }
        public Node(Node parent, string name)
        {
            Name = name;

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
        public int NumChildNodes
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
        public int NumNodeInputs
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
        public int NumNodeOutputs
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
        public int NumNodeParameters
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

        internal override void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WritePropertyName(typeof(Node).FullName);
            writer.WriteStartObject();

            writer.WriteString("Name", Name);

            writer.WriteStartArray("NodeInputs");
            foreach (var nodeInput in m_nodeInputs)
            {
                JsonSerializer.Serialize(writer, nodeInput, nodeInput.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("NodeOutputs");
            foreach (var nodeOutput in m_nodeOutputs)
            {
                JsonSerializer.Serialize(writer, nodeOutput, nodeOutput.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("NodeParameters");
            foreach (var nodeParameter in m_nodeParameters)
            {
                JsonSerializer.Serialize(writer, nodeParameter, nodeParameter.GetType(), options);
            }
            writer.WriteEndArray();

            writer.WriteStartArray("ChildNodes");
            foreach (var childNode in m_childNodes)
            {
                JsonSerializer.Serialize(writer, childNode, childNode.GetType(), options);
            }
            writer.WriteEndArray();

            // When adding more properties make sure to add them here.

            writer.WriteEndObject();
        }
        internal override void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Serialization.Expect(ref reader, JsonTokenType.PropertyName, typeof(Node).FullName);
            Serialization.Expect(ref reader, JsonTokenType.StartObject);
            Serialization.Expect(ref reader, JsonTokenType.PropertyName, "Name");
            Serialization.Expect(ref reader, JsonTokenType.String);
            Name = reader.GetString();

            Serialization.Expect(ref reader, JsonTokenType.PropertyName, "NodeInputs");
            Serialization.Expect(ref reader, JsonTokenType.StartArray);
            while(reader.TokenType != JsonTokenType.EndArray)
            {
                Serialization.Expect(ref reader, JsonTokenType.StartObject);
                var nodeInput = JsonSerializer.Deserialize<NodeInput>(ref reader, options);
                nodeInput.Node = this;
                m_nodeInputs.Add(nodeInput);
            }

            Serialization.Expect(ref reader, JsonTokenType.PropertyName, "NodeOutputs");
            Serialization.Expect(ref reader, JsonTokenType.StartArray);
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Serialization.Expect(ref reader, JsonTokenType.StartObject);
                var nodeOutput = JsonSerializer.Deserialize<NodeOutput>(ref reader, options);
                nodeOutput.Node = this;
                m_nodeOutputs.Add(nodeOutput);
            }

            Serialization.Expect(ref reader, JsonTokenType.PropertyName, "NodeParameters");
            Serialization.Expect(ref reader, JsonTokenType.StartArray);
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Serialization.Expect(ref reader, JsonTokenType.StartObject);
                var nodeParameter = JsonSerializer.Deserialize<NodeParameter>(ref reader, options);
                nodeParameter.Node = this;
                m_nodeParameters.Add(nodeParameter);
            }

            Serialization.Expect(ref reader, JsonTokenType.PropertyName, "ChildNodes");
            Serialization.Expect(ref reader, JsonTokenType.StartArray);
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Serialization.Expect(ref reader, JsonTokenType.StartObject);
                var node = JsonSerializer.Deserialize<Node>(ref reader, options);
                node.ParentNode = this;
                m_childNodes.Add(node);
            }
            Serialization.FinishCurrentObject(ref reader);
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
