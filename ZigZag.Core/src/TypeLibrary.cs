using System;
using System.Collections.Generic;
using System.Linq;
using ZigZag.Core.Parameters;


namespace ZigZag.Core
{
    public static class TypeLibrary
    {
        public class TypeNameTakenException : Exception { }
        public class UnknownTypeException : Exception { }

        static TypeLibrary()
        {
            AddType(typeof(FloatParameter));
            AddType(typeof(IntParameter));
        }

        public static void AddType(Type type)
        {
            if (type.IsSubclassOf(typeof(ZObject)))
            {
                if (m_types.ContainsKey(type.FullName))
                {
                    throw new TypeNameTakenException();
                }
                else
                {
                    m_types.Add(type.FullName, type);
                }
            }
        }

        public static void AddTypes(IEnumerable<Type> types)
        {
            foreach(var type in types)
            {
                AddType(type);
            }
        }

        public static T CreateInstance<T>(string nodeTypeName)
        {
            if (m_types.ContainsKey(nodeTypeName))
            {
                var type = m_types[nodeTypeName];
                var resultType = typeof(T);

                if (type.IsSubclassOf(resultType) || type.Equals(resultType))
                {
                    return (T)Activator.CreateInstance(type);
                }
            }
            throw new UnknownTypeException();
        }

        public static Type GetType(string nodeTypeName)
        {
            if (m_types.ContainsKey(nodeTypeName))
            {
                return m_types[nodeTypeName];
            }
            return null;
        }

        public static IEnumerable<Type> Types
        {
            get
            {
                return m_types.Select(kv => kv.Value);
            }
        }

        private static readonly Dictionary<string, Type> m_types = new Dictionary<string, Type>();
    }
}
