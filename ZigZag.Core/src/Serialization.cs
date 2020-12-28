using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace ZigZag.Core
{
    public static class Serialization
    {
        public class UnknownTypeException : JsonException
        {
            public UnknownTypeException(string typeName)
                : base($"Attempt to instantiate unknown type: {typeName}") { }

            public UnknownTypeException(string typeName, TypeLibrary.UnknownTypeException inner) 
                : base($"Attempt to instantiate unknown type: {typeName}", inner) { }
        }

        public static void Assert(bool statement)
        {
            if (!statement)
            {
                throw new JsonException();
            }
        }

        public static void FinishCurrentObject(ref Utf8JsonReader reader)
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

        public static void Expect(ref Utf8JsonReader reader, JsonTokenType expectedToken)
        {
            reader.Read();
            if (reader.TokenType != expectedToken)
            {
                throw new JsonException();
            }
        }

        public static void Expect(ref Utf8JsonReader reader, JsonTokenType expectedToken, string expectedValue)
        {
            reader.Read();
            if (reader.TokenType != expectedToken)
            {
                throw new JsonException();
            }
            if (reader.GetString() != expectedValue)
            {
                throw new JsonException();
            }
        }

        internal static void WriteNode(Node node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {

        }

        internal static void ReadNode(Node node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            /*
            Assert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.PropertyName);
            Assert(reader.GetString() == "Name");
            
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Null);
            if (reader.TokenType == JsonTokenType.String)
            {
                node.Name = reader.GetString();
            }

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.PropertyName);
            Assert(reader.GetString() == "Children");

            reader.Read();
            Assert(reader.TokenType == JsonTokenType.StartArray);

            reader.Read();

            if (reader.TokenType != JsonTokenType.EndArray)
            {
                node.m_childNodes = new List<Node>();
            }

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Assert(reader.TokenType == JsonTokenType.StartObject);

                var child = JsonSerializer.Deserialize<Node>(ref reader, options);
                child.ParentNode = node;
                node.m_childNodes.Add(child);

                Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }
            Assert(reader.TokenType == JsonTokenType.EndArray);

            // Forward compatible as long as new properties are added at the end.
            ReadTillEndOfObject(ref reader);

            Assert(reader.TokenType == JsonTokenType.EndObject);*/
        }

        internal static void WriteNodeInput(NodeInput node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        internal static void ReadNodeInput(NodeInput node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.EndObject);
        }

        internal static void WriteNodeOutput(NodeOutput node, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        internal static void ReadNodeOutput(NodeOutput node, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Assert(reader.TokenType == JsonTokenType.StartObject);
            reader.Read();
            Assert(reader.TokenType == JsonTokenType.EndObject);
        }
    }
}
