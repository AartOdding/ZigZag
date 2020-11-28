using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ZigZag.Runtime
{
    internal static class AssemblyReader
    {

        public static List<Type> ReadProcessNodes(List<Assembly> assemblies)
        {
            List<Type> types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.IsSubclassOf(typeof(ZigZag.ProcessNode)))
                    {
                        types.Add(type);
                    }
                }
            }
            return types;
        }

        public static List<Type> ReadProcessNodes(Assembly assembly)
        {
            List<Type> types = new List<Type>();

            foreach (var t in assembly.ExportedTypes)
            {
                if (t.IsSubclassOf(typeof(ZigZag.ProcessNode)))
                {
                    types.Add(t);
                }
            }
            return types;
        }

    }
}
