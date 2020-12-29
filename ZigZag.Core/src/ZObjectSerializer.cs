using System;
using System.Collections.Generic;
using System.Reflection;
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

        private delegate void ReadJsonDelegate(ref Utf8JsonReader reader, JsonSerializerOptions options);


        public override ZObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Serialization.Assert(reader.TokenType == JsonTokenType.StartObject);

            Serialization.MustReadPropertyName(ref reader, JsonTokenType.PropertyName, "Type");
            Serialization.MustReadTokenType(ref reader, JsonTokenType.String);

            ZObject obj;

            try
            {
                obj = TypeLibrary.CreateInstance<ZObject>(reader.GetString());
            }
            catch (TypeLibrary.UnknownTypeException e)
            {
                throw new Serialization.UnknownTypeException(reader.GetString(), e);
            }

            Serialization.MustReadPropertyName(ref reader, JsonTokenType.PropertyName, "ID");
            Serialization.MustReadTokenType(ref reader, JsonTokenType.Number);
            var id = reader.GetInt64();

            var readJsonDelegates = GetReadJsonDelegates(obj);

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                Serialization.MustReadTokenType(ref reader, JsonTokenType.PropertyName);
                var typeName = reader.GetString();
                Serialization.MustReadTokenType(ref reader, JsonTokenType.StartObject);

                if (readJsonDelegates.ContainsKey(typeName))
                {
                    readJsonDelegates[typeName].Invoke(ref reader, options);
                }
                else
                {
                    // Warn
                    Serialization.FinishCurrentObject(ref reader);
                }
                Serialization.Assert(reader.TokenType == JsonTokenType.EndObject);
                reader.Read();
            }

            Serialization.MustReadTokenType(ref reader, JsonTokenType.EndObject);
            return obj;
        }

        private Dictionary<string, ReadJsonDelegate> GetReadJsonDelegates(ZObject obj)
        {
            var type = obj.GetType();
            var result = new Dictionary<string, ReadJsonDelegate>();

            while (type != typeof(ZObject))
            {
                var readJsonMethod = type.GetMethod("ReadJson", readJsonArgumentTypes);

                if (!(readJsonMethod is null) && readJsonMethod.DeclaringType == type)
                {
                    result.Add(type.FullName, (ReadJsonDelegate)Delegate.CreateDelegate(typeof(ReadJsonDelegate), obj, readJsonMethod));
                }

                type = type.BaseType;
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, ZObject obj, JsonSerializerOptions options)
        {
            var type = obj.GetType();
            var writeJsonArguments = new object[] { writer, options };

            writer.WriteStartObject();
            writer.WriteString("Type", type.FullName);
            writer.WriteNumber("ID", obj.ID);

            while (type != typeof(ZObject))
            {
                var writeJsonMethod = type.GetMethod("WriteJson", writeJsonArgumentTypes);

                if (!(writeJsonMethod is null) && type == writeJsonMethod.DeclaringType)
                {
                    writer.WritePropertyName(type.FullName);
                    writeJsonMethod.Invoke(obj, writeJsonArguments);
                }
                type = type.BaseType;
            }

            writer.WriteEndObject();
        }

        private static readonly Type[] writeJsonArgumentTypes = { typeof(Utf8JsonWriter), typeof(JsonSerializerOptions) };
        private static readonly Type[] readJsonArgumentTypes = { typeof(Utf8JsonReader).MakeByRefType(), typeof(JsonSerializerOptions) };
    }
}
