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

            writer.WriteEndObject();
        }

        internal static void ReadAbstractNodePart(AbstractNode node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                Assert(reader.TokenType == JsonTokenType.PropertyName);
                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "name":
                        Assert(reader.TokenType == JsonTokenType.String);
                        node.Name = reader.GetString();
                        reader.Read();
                        break;

                    case "children":
                        Assert(reader.TokenType == JsonTokenType.StartArray);
                        reader.Read();

                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            Assert(reader.TokenType == JsonTokenType.StartObject);

                            var child = JsonSerializer.Deserialize<AbstractNode>(ref reader, options);
                            child.Parent = node;

                            Assert(reader.TokenType == JsonTokenType.EndObject);
                        }
                        Assert(reader.TokenType == JsonTokenType.EndArray);
                        reader.Read();
                        break;

                    default:
                        if (reader.TokenType == JsonTokenType.StartArray ||
                            reader.TokenType == JsonTokenType.StartObject)
                        {
                            reader.Skip();
                        }
                        reader.Read();
                        break;
                }
            }

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
