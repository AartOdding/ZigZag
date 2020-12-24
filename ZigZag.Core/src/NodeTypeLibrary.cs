using System;
using System.Collections.Generic;
using System.Linq;


namespace ZigZag.Core
{
    public static class NodeTypeLibrary
    {
        public class NodeTypeNameTakenException : Exception { }
        public class NodeTypeMissingException : Exception { }

        public static void AddNodeType(Type nodeType)
        {
            if (nodeType.IsSubclassOf(typeof(Node)))
            {
                if (m_nodeTypes.ContainsKey(nodeType.FullName))
                {
                    throw new NodeTypeNameTakenException();
                }
                else
                {
                    m_nodeTypes.Add(nodeType.FullName, nodeType);
                }
            }
        }

        public static void AddNodeTypes(IEnumerable<Type> nodeTypes)
        {
            foreach(var t in nodeTypes)
            {
                AddNodeType(t);
            }
        }

        public static Node CreateNode(string nodeTypeName)
        {
            if (m_nodeTypes.ContainsKey(nodeTypeName))
            {
                return (Node)Activator.CreateInstance(m_nodeTypes[nodeTypeName]);
            }
            else
            {
                throw new NodeTypeMissingException();
            }
        }

        public static Type GetNodeType(string nodeTypeName)
        {
            if (m_nodeTypes.ContainsKey(nodeTypeName))
            {
                return m_nodeTypes[nodeTypeName];
            }
            return null;
        }

        public static IEnumerable<Type> NodeTypes
        {
            get
            {
                return m_nodeTypes.Select(kv => kv.Value);
            }
        }

        private static Dictionary<string, Type> m_nodeTypes = new Dictionary<string, Type>();
    }
}
