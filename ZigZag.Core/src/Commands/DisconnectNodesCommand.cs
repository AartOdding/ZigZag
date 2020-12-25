

namespace ZigZag.Core.Commands
{
    public class DisconnectNodesCommand : AbstractCommand
    {
        public DisconnectNodesCommand(NodeOutput output, NodeInput input)
        {
            m_output = output;
            m_input = input;
        }

        internal override void Do()
        {
            if (m_input is null || m_output is null)
            {
                throw new CommandException();
            }
            if (m_input.ConnectedOutput == m_output &&
                m_output.m_connectedNodeInputs.Contains(m_input))
            {
                m_output.m_connectedNodeInputs.Remove(m_input);
                m_input.ConnectedOutput = null;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_input.ConnectedOutput = m_output;
            m_output.m_connectedNodeInputs.Add(m_input);
        }

        private readonly NodeOutput m_output;
        private readonly NodeInput m_input;
    }
}
