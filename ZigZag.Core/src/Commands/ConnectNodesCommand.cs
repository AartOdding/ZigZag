

namespace ZigZag.Core.Commands
{
    public class ConnectNodesCommand : AbstractCommand
    {
        public ConnectNodesCommand(OutputNode output, InputNode input)
        {
            m_outputNode = output;
            m_inputNode = input;
        }

        internal override void Do()
        {
            if (m_inputNode is null || m_outputNode is null)
            {
                throw new CommandException();
            }
            if (m_inputNode.ConnectedOutputNode is null &&
                !m_outputNode.m_connectedInputNodes.Contains(m_inputNode))
                // And check scope is compatible!
            {
                m_inputNode.ConnectedOutputNode = m_outputNode;
                m_outputNode.m_connectedInputNodes.Add(m_inputNode);
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_outputNode.m_connectedInputNodes.Remove(m_inputNode);
            m_inputNode.ConnectedOutputNode = null;
        }

        private readonly OutputNode m_outputNode;
        private readonly InputNode m_inputNode;
    }
}
