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
            JsonAssert(reader.GetString() == "ZigZag.Core.AbstractNode");

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
                case InputNode inputNode:
                    JsonAssert(nodeBaseClass == "ZigZag.Core.InputNode");
                    Serialization.ReadInputNodePart(inputNode, ref reader, options);
                    break;
                case OutputNode outputNode:
                    JsonAssert(nodeBaseClass == "ZigZag.Core.OutputNode");
                    Serialization.ReadOutputNodePart(outputNode, ref reader, options);
                    break;
                case ProcessNode processNode:
                    JsonAssert(nodeBaseClass == "ZigZag.Core.ProcessNode");
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

            writer.WritePropertyName("ZigZag.Core.AbstractNode");
            Serialization.WriteAbstractNodePart(node, writer, options);

            switch (node)
            {
                case InputNode inputNode:
                    writer.WritePropertyName("ZigZag.Core.InputNode");
                    Serialization.WriteInputNodePart(inputNode, writer, options);
                    break;
                case OutputNode outputNode:
                    writer.WritePropertyName("ZigZag.Core.OutputNode");
                    Serialization.WriteOutputNodePart(outputNode, writer, options);
                    break;
                case ProcessNode processNode:
                    writer.WritePropertyName("ZigZag.Core.ProcessNode");
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
