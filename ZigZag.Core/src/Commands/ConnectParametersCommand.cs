using ZigZag.Core.Parameters;


namespace ZigZag.Core.Commands
{
    public class ConnectParametersCommand : AbstractCommand
    {
        public ConnectParametersCommand(NodeParameter listened, NodeParameter listener)
        {
            m_listened = listened;
            m_listener = listener;
        }

        internal override void Do()
        {
            if (m_listener is null || m_listened is null ||
                m_listener.Listened is not null ||
                m_listened.ContainsListener(m_listener))
            {
                throw new CommandException();
            }
            else
            {
                m_listener.Listened = m_listened;
                m_listened.AddListener(m_listener);
            }
        }

        internal override void Undo()
        {
            m_listened.RemoveListener(m_listener);
            m_listener.Listened = null;
        }

        private readonly NodeParameter m_listened;
        private readonly NodeParameter m_listener;
    }
}
