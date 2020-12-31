using ZigZag.Core.Parameters;


namespace ZigZag.Core.Commands
{
    public class DisconnectParametersCommand : AbstractCommand
    {
        public DisconnectParametersCommand(NodeParameter listened, NodeParameter listener)
        {
            m_listened = listened;
            m_listener = listener;
        }

        internal override void Do()
        {
            if (m_listener is null || m_listened is null ||
                m_listener.Listened != m_listened ||
                !m_listened.ContainsListener(m_listener))
            {
                throw new CommandException();
            }
            else
            {
                m_listened.RemoveListener(m_listener);
                m_listener.Listened = m_listened;
            }
        }

        internal override void Undo()
        {
            m_listener.Listened = m_listened;
            m_listened.AddListener(m_listener);
        }

        private readonly NodeParameter m_listened;
        private readonly NodeParameter m_listener;
    }
}
