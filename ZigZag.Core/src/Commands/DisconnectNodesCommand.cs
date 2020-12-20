

namespace ZigZag.Core.Commands
{
    public class DisconnectNodesCommand : AbstractCommand
    {
        public DisconnectNodesCommand(OutputNode output, InputNode input)
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
            if (m_inputNode.ConnectedOutputNode == m_outputNode &&
                m_outputNode.m_connectedInputNodes.Contains(m_inputNode))
            {
                m_outputNode.m_connectedInputNodes.Remove(m_inputNode);
                m_inputNode.ConnectedOutputNode = null;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_inputNode.ConnectedOutputNode = m_outputNode;
            m_outputNode.m_connectedInputNodes.Add(m_inputNode);
        }

        private readonly OutputNode m_outputNode;
        private readonly InputNode m_inputNode;
    }
}
