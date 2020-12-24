using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core
{
    public class NodeSerializer : JsonConverter<AbstractNode>
    {
        class UnknownNodeTypeException : Exception
        {
            public UnknownNodeTypeException(string nodeType)
            {
                NodeType = nodeType;
            }

            public string NodeType { get; }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(AbstractNode))
                || typeToConvert.Equals(typeof(AbstractNode));
        }

        public override AbstractNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonAssert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            JsonAssert(reader.GetString() == "Type");

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.String);

            string nodeTypeString = reader.GetString();
            var nodeType = NodeTypeLibrary.GetNodeType(nodeTypeString);

            if (nodeType is null)
            {
                throw new UnknownNodeTypeException(nodeTypeString);
            }

            AbstractNode node = (AbstractNode)Activator.CreateInstance(nodeType);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            JsonAssert(reader.GetString() == typeof(AbstractNode).FullName);

            reader.Read();

            JsonAssert(reader.TokenType == JsonTokenType.StartObject);
            Serialization.ReadAbstractNodePart(node, ref reader, options);
            JsonAssert(reader.TokenType == JsonTokenType.EndObject);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            string nodeBaseClass = reader.GetString();

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.StartObject);

            switch (node)
            {
                case NodeInput nodeInput:
                    JsonAssert(nodeBaseClass == typeof(NodeInput).FullName);
                    Serialization.ReadNodeInputPart(nodeInput, ref reader, options);
                    break;
                case NodeOutput nodeOutput:
                    JsonAssert(nodeBaseClass == typeof(NodeOutput).FullName);
                    Serialization.ReadNodeOutputPart(nodeOutput, ref reader, options);
                    break;
                case ProcessNode processNode:
                    JsonAssert(nodeBaseClass == typeof(ProcessNode).FullName);
                    Serialization.ReadProcessNodePart(processNode, ref reader, options);
                    break;
            }

            JsonAssert(reader.TokenType == JsonTokenType.EndObject);
            reader.Read();

            Serialization.ReadTillEndOfObject(ref reader);
            JsonAssert(reader.TokenType == JsonTokenType.EndObject);

            return node;
        }

        public override void Write(Utf8JsonWriter writer, AbstractNode node, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString("Type", node.GetType().FullName);

            writer.WritePropertyName(typeof(AbstractNode).FullName);
            Serialization.WriteAbstractNodePart(node, writer, options);

            switch (node)
            {
                case NodeInput nodeInput:
                    writer.WritePropertyName(typeof(NodeInput).FullName);
                    Serialization.WriteInputNodePart(nodeInput, writer, options);
                    break;
                case NodeOutput nodeOutput:
                    writer.WritePropertyName(typeof(NodeOutput).FullName);
                    Serialization.WriteOutputNodePart(nodeOutput, writer, options);
                    break;
                case ProcessNode processNode:
                    writer.WritePropertyName(typeof(ProcessNode).FullName);
                    Serialization.WriteProcessNodePart(processNode, writer, options);
                    break;
            }

            writer.WriteEndObject();
        }

        private void JsonAssert(bool statement)
        {
            if (!statement)
            {
                throw new JsonException();
            }
        }
    }
}
