using System.Collections.Generic;
using System.Diagnostics;


namespace ZigZag.Core
{
    public abstract class NodeOutput : ZObject
    {
        public NodeOutput()
        {
        }

        public NodeOutput(Node node)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            Node = node;
        }

        public NodeOutput(string name)
        {
            Name = name;
        }

        public NodeOutput(Node node, string name)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            Node = node;
            Name = name;
        }

        public string Name
        {
            get;
            internal set;
        }

        public Node Node
        {
            get;
            internal set;
        }

        public abstract void Update();

        public IEnumerable<NodeInput> ConnectedNodeInputs
        {
            get
            {
                return m_connectedNodeInputs;
            }
        }

        internal readonly List<NodeInput> m_connectedNodeInputs = new List<NodeInput>();
    }
}
