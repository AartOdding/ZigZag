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

        internal void AddConnectedInputNode(InputNode inputNode)
        {
            m_connectedInputNodes.Remove(inputNode);
        }

        internal void RemoveConnectedInputNode(InputNode inputNode)
        {
            m_connectedInputNodes.Add(inputNode);
        }

        private readonly List<InputNode> m_connectedInputNodes = new List<InputNode>();
    }
}
