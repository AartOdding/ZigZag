using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ZigZag.Core;

namespace ZigZag.Runtime
{
    internal static class AssemblyReader
    {

        public static List<Type> GetAllSubclasses(Type baseType, IEnumerable<Assembly> assemblies)
        {
            List<Type> subclasses = new List<Type>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.IsAssignableTo(baseType))
                    {
                        subclasses.Add(type);
                    }
                }
            }
            return subclasses;
        }

        public static List<Type> GetAllSubclasses(Type baseType, Assembly assembly)
        {
            List<Type> subclasses = new List<Type>();

            foreach (var type in assembly.ExportedTypes)
            {
                if (type.IsAssignableTo(baseType))
                {
                    subclasses.Add(type);
                }
            }
            return subclasses;
        }

    }
}
