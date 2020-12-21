using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace ZigZag.Core
{
    public static class Serialization
    {
        public static void Assert(bool statement)
        {
            if (!statement)
            {
                throw new JsonException();
            }
        }

        public static void ReadTillEndOfObject(ref Utf8JsonReader reader)
        {
            while (reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.StartArray ||
                    reader.TokenType == JsonTokenType.StartObject)
                {
                    reader.Skip();
                }
                reader.Read();
            }
        }

        internal static void WriteAbstractNodePart(AbstractNode node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("name", node.Name);

            writer.WriteStartArray("children");

            foreach (var child in node.Children)
            {
                JsonSerializer.Serialize(writer, child, child.GetType(), options);
            }

            writer.WriteEndArray();

            // When adding more properties make sure to add them here.

            writer.WriteEndObject();
        }

        internal static void ReadAbstractNodePart(AbstractNode node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.PropertyName);
            Assert(reader.GetString() == "name");
            
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Null);
            if (reader.TokenType == JsonTokenType.String)
            {
                node.Name = reader.GetString();
            }

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.PropertyName);
            Assert(reader.GetString() == "children");

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.StartArray);

            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                node.m_children = new List<AbstractNode>();
            }

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Assert(reader.TokenType == JsonTokenType.StartObject);

                var child = JsonSerializer.Deserialize<AbstractNode>(ref reader, options);
                child.Parent = node;
                node.m_children.Add(child);

                Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }
            Assert(reader.TokenType == JsonTokenType.EndArray);

            // Forward compatible as long as new properties are added at the end.
            ReadTillEndOfObject(ref reader);

            Assert(reader.TokenType == JsonTokenType.EndObject);
        }

        internal static void WriteInputNodePart(InputNode node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        internal static void ReadInputNodePart(InputNode node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.EndObject);
        }

        internal static void WriteOutputNodePart(OutputNode node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        internal static void ReadOutputNodePart(OutputNode node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.EndObject);
        }

        internal static void WriteProcessNodePart(ProcessNode node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        internal static void ReadProcessNodePart(ProcessNode node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.EndObject);
        }
    }
}
