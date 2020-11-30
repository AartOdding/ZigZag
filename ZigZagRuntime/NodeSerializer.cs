using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZigZag.Runtime
{
    internal class NodeSerializer : JsonConverter<AbstractNode>
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


        // requirements of a json node:
        // node must start be its own object
        // node must specify its type as a string first IE
        // "type": "ZigZag.Text.Print"
        // for now the rest is not mandatory

        // create a way to add new fields after name, and before children
        // and make sure works forward and backwards compatible.

        public override AbstractNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonAssert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            JsonAssert(reader.GetString() == "type");

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.String);

            string nodeTypeString = reader.GetString();
            var nodeType = TypeLibrary.GetProcessNode(nodeTypeString);

            AbstractNode node;

            if (nodeType is null)
            {
                throw new UnknownNodeTypeException(nodeTypeString);
            }
            else
            {
                node = (AbstractNode)Activator.CreateInstance(nodeType);

                // while loop until we reach a property named children

                // after children have been made set this as parent

                reader.Read();
                JsonAssert(reader.TokenType == JsonTokenType.PropertyName);


            }
            return node;
        }

        public override void Write(Utf8JsonWriter writer, AbstractNode node, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("type", node.GetType().FullName);
            writer.WriteString("name", node.Name);

            writer.WriteStartArray("children");

            foreach(var child in node.Children)
            {
                JsonSerializer.Serialize(writer, child, child.GetType(), options);
            }

            writer.WriteEndArray();
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
