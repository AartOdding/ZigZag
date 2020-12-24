

namespace ZigZag.Core.Commands
{
    public class ConnectNodesCommand : AbstractCommand
    {
        public ConnectNodesCommand(NodeOutput output, NodeInput input)
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
            if (m_input.ConnectedNodeOutput is null &&
                !m_output.m_connectedNodeInputs.Contains(m_input))
                // And check scope is compatible!
            {
                m_input.ConnectedNodeOutput = m_output;
                m_output.m_connectedNodeInputs.Add(m_input);
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_output.m_connectedNodeInputs.Remove(m_input);
            m_input.ConnectedNodeOutput = null;
        }

        private readonly NodeOutput m_output;
        private readonly NodeInput m_input;
    }
}
