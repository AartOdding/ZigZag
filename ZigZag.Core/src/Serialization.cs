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

        public static void MustReadTokenType(ref Utf8JsonReader reader, JsonTokenType expectedToken)
        {
            reader.Read();
            if (reader.TokenType != expectedToken)
            {
                throw new JsonException();
            }
        }

        public static void MustReadPropertyName(ref Utf8JsonReader reader, JsonTokenType expectedToken, string expectedValue)
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
    }
}
