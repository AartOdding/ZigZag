using System;
using System.Collections.Generic;
using System.Linq;


namespace ZigZag.Core
{
    public static class NodeLibrary
    {

        public static void AddNodeType(Type processNode)
        {
            m_processNodes.Add(processNode.FullName, processNode);
        }

        public static void AddNodeTypes(List<Type> processNodes)
        {
            foreach(var t in processNodes)
            {
                m_processNodes.Add(t.FullName, t);
            }
        }

        public static Type GetNodeType(string fullName)
        {
            return m_processNodes[fullName];
        }

        public static IEnumerable<Type> ProcessNodeTypes
        {
            get
            {
                return m_processNodes.Select(kv => kv.Value);
            }
        }

        private static Dictionary<string, Type> m_processNodes = new Dictionary<string, Type>();

    }
}
