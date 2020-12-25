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

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.PropertyName);
            Serialization.Assert(reader.GetString() == typeof(Node).FullName);

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);
            Serialization.ReadNode(node, ref reader, options);
            Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.PropertyName);
            string nodeBaseClass = reader.GetString();

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);

            switch (node)
            {
                case NodeInput nodeInput:
                    Serialization.Assert(nodeBaseClass == typeof(NodeInput).FullName);
                    Serialization.ReadNodeInput(nodeInput, ref reader, options);
                    break;
                case NodeOutput nodeOutput:
                    Serialization.Assert(nodeBaseClass == typeof(NodeOutput).FullName);
                    Serialization.ReadNodeOutput(nodeOutput, ref reader, options);
                    break;
            }

            Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);
            reader.Read();

            Serialization.ReadTillEndOfObject(ref reader);
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
