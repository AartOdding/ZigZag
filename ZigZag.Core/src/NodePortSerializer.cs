using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace ZigZag.Core
{
    public class NodePortSerializer : JsonConverter<NodePort>
    {

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(NodePort))
                || typeToConvert.Equals(typeof(NodePort));
        }

        public override NodePort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.PropertyName);
            Serialization.Assert(reader.GetString() == "Type");

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.String);

            NodePort nodePort;

            try
            {
                nodePort = TypeLibrary.CreateInstance<NodePort>(reader.GetString());
            }
            catch(TypeLibrary.UnknownTypeException e)
            {
                throw new Serialization.UnknownTypeException(reader.GetString(), e);
            }

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.PropertyName);
            Serialization.Assert(reader.GetString() == typeof(NodePort).FullName);

            reader.Read();
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);
            Serialization.ReadNodePort(nodePort, ref reader, options);
            Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);



            return nodePort;
        }

        public override void Write(Utf8JsonWriter writer, NodePort nodePort, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Type", nodePort.GetType().FullName);

            writer.WritePropertyName(typeof(NodePort).FullName);
            Serialization.WriteNodePort(nodePort, writer, options);
        }
    }
}
