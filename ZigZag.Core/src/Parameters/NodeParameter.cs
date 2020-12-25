using System.Collections.Generic;


namespace ZigZag.Core.Parameters
{
    public abstract class NodeParameter : ZObject
    {
        public NodeParameter()
        {
        }

        public NodeParameter(Node node)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            ParentNode = node;
        }

        public NodeParameter(string name)
        {
            Name = name;
        }

        public NodeParameter(Node node, string name)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            ParentNode = node;
            Name = name;
        }

        public string Name
        {
            get;
            internal set;
        }

        public Node ParentNode
        {
            get;
            internal set;
        }

        public abstract void Update();

        public abstract bool Accepts(NodeParameter parameter);

        public bool Changed
        {
            get;
            protected set;
        }

        public bool IsListening()
        {
            return !(ListenedParameter is null);
        }

        internal void SetChanged(bool changed)
        {
            Changed = changed;
        }

        public NodeParameter ListenedParameter
        {
            get;
            internal set;
        }

        public IEnumerable<NodeParameter> ListeningParameters
        {
            get
            {
                return m_listeningParameters;
            }
        }

        internal NodeParameter m_listenedParameter;
        internal readonly List<NodeParameter> m_listeningParameters = new List<NodeParameter>();
    }
}
