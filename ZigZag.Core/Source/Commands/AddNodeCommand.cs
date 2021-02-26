

namespace ZigZag.Core.Commands
{
    public class AddNodeCommand : AbstractCommand
    {
        public AddNodeCommand(Node parentNode, Node childNodeToAdd)
        {
            m_parentNode = parentNode;
            m_childNode = childNodeToAdd;
        }

        internal override void Do()
        {
            if (m_parentNode is null || m_childNode is null)
            {
                throw new CommandException();
            }
            if (m_childNode.ParentNode is null &&
                //!m_parentNode.m_childNodes.Contains(m_childNode) &&
                !m_parentNode.IsIndirectChildOf(m_childNode))
            {
                m_parentNode.m_childNodes.Add(m_childNode);
                m_childNode.ParentNode = m_parentNode;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_parentNode.m_childNodes.Remove(m_childNode);
            m_childNode.ParentNode = null;
        }

        private readonly Node m_parentNode;
        private readonly Node m_childNode;
    }
}
