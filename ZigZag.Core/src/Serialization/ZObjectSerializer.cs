using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core.Serialization
{
    public class ZObjectSerializer : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(ZObject))
                || typeToConvert.Equals(typeof(ZObject));
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            Type converterType = typeof(ZObjectSerializerInner<>).MakeGenericType(new Type[] { type });
            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class ZObjectSerializerInner<T> : JsonConverter<T> where T : ZObject
        {
            public override bool CanConvert(Type typeToConvert)
            {
                return typeToConvert.IsSubclassOf(typeof(ZObject))
                    || typeToConvert.Equals(typeof(ZObject));
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.StartObject);

                SerializationUtils.MustReadPropertyName(ref reader, "Type");
                SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.String);

                T obj;

                try
                {
                    obj = TypeLibrary.CreateInstance<T>(reader.GetString());
                }
                catch (TypeLibrary.UnknownTypeException e)
                {
                    throw new SerializationUtils.UnknownTypeException(reader.GetString(), e);
                }

                SerializationUtils.MustReadPropertyName(ref reader, "ID");
                SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.Number);
                var id = reader.GetInt64();

                var readJsonDelegates = GetReadJsonDelegates(obj);

                reader.Read();
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    SerializationUtils.Assert(reader.TokenType == JsonTokenType.PropertyName);
                    var typeName = reader.GetString();
                    SerializationUtils.MustReadTokenType(ref reader, JsonTokenType.StartObject);

                    if (readJsonDelegates.ContainsKey(typeName))
                    {
                        readJsonDelegates[typeName].Invoke(ref reader, options);
                    }
                    else
                    {
                        // Warn
                        SerializationUtils.FinishCurrentObject(ref reader);
                    }
                    SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                    reader.Read();
                }

                SerializationUtils.Assert(reader.TokenType == JsonTokenType.EndObject);
                return obj;
            }

            public override void Write(Utf8JsonWriter writer, T obj, JsonSerializerOptions options)
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

            private delegate void ReadJsonDelegate(ref Utf8JsonReader reader, JsonSerializerOptions options);

            private static readonly Type[] writeJsonArgumentTypes = { typeof(Utf8JsonWriter), typeof(JsonSerializerOptions) };
            private static readonly Type[] readJsonArgumentTypes = { typeof(Utf8JsonReader).MakeByRefType(), typeof(JsonSerializerOptions) };
        }
    }
}
