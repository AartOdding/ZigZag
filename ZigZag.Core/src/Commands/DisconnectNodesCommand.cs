 

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
            if (m_input is null || m_output is null ||
                m_input.ConnectedOutput != m_output ||
                !m_output.ContainsConnectedInput(m_input))
            {
                throw new CommandException();
            }
            else
            {
                m_output.RemoveConnectedInput(m_input);
                m_input.ConnectedOutput = null;
            }
        }

        internal override void Undo()
        {
            m_input.ConnectedOutput = m_output;
            m_output.AddConnectedInput(m_input);
        }

        private readonly NodeOutput m_output;
        private readonly NodeInput m_input;
    }
}
