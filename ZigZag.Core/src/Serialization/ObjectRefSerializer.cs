using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core.Serialization
{
    public class ObjectRefSerializer : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && 
                typeToConvert.GetGenericTypeDefinition() == typeof(ObjectRef<>);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            Type objectType = type.GetGenericArguments()[0];
            Type converterType = typeof(ObjectRefSerializerInner<>).MakeGenericType(new Type[] { objectType });
            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class ObjectRefSerializerInner<T> : JsonConverter<ObjectRef<T>> where T : ZObject
        {
            public override bool CanConvert(Type typeToConvert)
            {
                return typeToConvert.IsGenericType &&
                    typeToConvert.GetGenericTypeDefinition() == typeof(ObjectRef<>);
            }

            public override ObjectRef<T> Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                SerializationUtils.Assert(reader.TokenType == JsonTokenType.Number || 
                                          reader.TokenType == JsonTokenType.Null);
                var objectRef = new ObjectRef<T>();

                if (reader.TokenType == JsonTokenType.Number)
                {
                    objectRef.DeserializedID = reader.GetInt64();
                }
                return objectRef;
            }

            public override void Write(
                Utf8JsonWriter writer,
                ObjectRef<T> objectRef,
                JsonSerializerOptions options)
            {
                if (objectRef.Object is null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteNumberValue(objectRef.Object.ID);
                }
            }
        }
    }
}
