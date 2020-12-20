using System.Collections.Generic;


namespace ZigZag.Core
{
    public abstract class OutputNode : AbstractNode
    {
        public IEnumerable<InputNode> ConnectedInputNodes
        {
            get
            {
                return m_connectedInputNodes;
            }
        }

        internal readonly List<InputNode> m_connectedInputNodes = new List<InputNode>();
    }
}
