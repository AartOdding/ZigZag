using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZigZag.Runtime
{
    internal static class TypeLibrary
    {

        public static void AddProcessNode(Type processNode)
        {
            m_processNodes.Add(processNode.FullName, processNode);
        }

        public static void AddProcessNodes(List<Type> processNodes)
        {
            foreach(var t in processNodes)
            {
                m_processNodes.Add(t.FullName, t);
            }
        }

        public static Type GetProcessNode(string fullName)
        {
            return m_processNodes[fullName];
        }

        public static IEnumerable<Type> ProcessNodes
        {
            get
            {
                return m_processNodes.Select(kv => kv.Value);
            }
        }

        private static Dictionary<string, Type> m_processNodes = new Dictionary<string, Type>();

    }
}
