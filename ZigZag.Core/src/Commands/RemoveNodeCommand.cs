

namespace ZigZag.Core.Commands
{
    public class RemoveNodeCommand : AbstractCommand
    {
        public RemoveNodeCommand(Node parentNode, Node childNodeToRemove)
        {
            m_parentNode = parentNode;
            m_childNode = childNodeToRemove;
        }

        internal override void Do()
        {
            if (m_parentNode is null || m_childNode is null)
            {
                throw new CommandException();
            }
            if (m_childNode.ParentNode == m_parentNode)// &&
                //m_parentNode.m_childNodes.Contains(m_childNode))
                // And check there are no connections
            {
                m_parentNode.RemoveChildNode(m_childNode);
                m_childNode.ParentNode = null;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_parentNode.AddChildNode(m_childNode);
            m_childNode.ParentNode = m_parentNode;
        }

        private readonly Node m_parentNode;
        private readonly Node m_childNode;
    }
}
