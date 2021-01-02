using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core.Serialization
{
    public class ObjectRefSerializer : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && 
                typeof(ZObject).IsAssignableFrom(typeToConvert.GenericTypeArguments[0]) &&
                typeToConvert.GetGenericTypeDefinition() == typeof(ObjectRef<>);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            Type objectType = type.GetGenericArguments()[0];
            Type converterType = typeof(ObjectRefSerializerInner<>).MakeGenericType(new Type[] { objectType });
            return (JsonConverter)Activator.CreateInstance(converterType, new object[] { m_createdObjectReferences });
        }

        public void ResolveObjects(Dictionary<long, ZObject> objects)
        {
            foreach (var kv in m_createdObjectReferences)
            {
                long id = kv.Key;

                if (objects.ContainsKey(id))
                {
                    ZObject obj = objects[id];

                    foreach (var objRef in kv.Value)
                    {
                        objRef.SetObject(obj);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private readonly Dictionary<long, List<IObjectRef>> m_createdObjectReferences = new Dictionary<long, List<IObjectRef>>();

        private class ObjectRefSerializerInner<T> : JsonConverter<ObjectRef<T>> where T : ZObject
        {
            public ObjectRefSerializerInner(Dictionary<long, List<IObjectRef>> createdObjectReferences)
            {
                m_createdObjectReferences = createdObjectReferences;
            }

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
                    long id = reader.GetInt64();

                    if (!m_createdObjectReferences.ContainsKey(id))
                    {
                        m_createdObjectReferences.Add(id, new List<IObjectRef>());
                    }
                    m_createdObjectReferences[id].Add(objectRef);
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

            private readonly Dictionary<long, List<IObjectRef>> m_createdObjectReferences;
        }
    }
}
