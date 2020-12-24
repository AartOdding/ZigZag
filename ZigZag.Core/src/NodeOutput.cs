using System.Collections.Generic;


namespace ZigZag.Core
{
    public abstract class NodeOutput : AbstractNode
    {
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
