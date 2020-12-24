using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core
{
    public class NodeSerializer : JsonConverter<Node>
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
            return typeToConvert.IsSubclassOf(typeof(Node))
                || typeToConvert.Equals(typeof(Node));
        }

        public override Node Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            Node node = (Node)Activator.CreateInstance(nodeType);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            JsonAssert(reader.GetString() == typeof(Node).FullName);

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
            }

            JsonAssert(reader.TokenType == JsonTokenType.EndObject);
            reader.Read();

            Serialization.ReadTillEndOfObject(ref reader);
            JsonAssert(reader.TokenType == JsonTokenType.EndObject);

            return node;
        }

        public override void Write(Utf8JsonWriter writer, Node node, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString("Type", node.GetType().FullName);

            writer.WritePropertyName(typeof(Node).FullName);
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
