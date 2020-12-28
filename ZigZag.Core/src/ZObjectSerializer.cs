using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core
{
    public class ZObjectSerializer : JsonConverter<ZObject>
    {

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(ZObject))
                || typeToConvert.Equals(typeof(ZObject));
        }

        public override ZObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.PropertyName);
            Serialization.Assert(reader.GetString() == "Type");

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.String);

            Node node;

            try
            {
                node = TypeLibrary.CreateInstance<Node>(reader.GetString());
            }
            catch (TypeLibrary.UnknownTypeException e)
            {
                throw new Serialization.UnknownTypeException(reader.GetString(), e);
            }

            node.ReadJson(ref reader, options);
            Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);
            
            return node;
        }

        public override void Write(Utf8JsonWriter writer, ZObject zobject, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", zobject.GetType().FullName);
            zobject.WriteJson(writer, options);
            writer.WriteEndObject();
        }

    }
}
