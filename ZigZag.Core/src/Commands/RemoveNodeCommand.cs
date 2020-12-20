

namespace ZigZag.Core.Commands
{
    public class RemoveNodeCommand : AbstractCommand
    {
        public RemoveNodeCommand(AbstractNode parentNode, AbstractNode childNodeToRemove)
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
            if (m_childNode.Parent == m_parentNode &&
                m_parentNode.m_children.Contains(m_childNode))
                // And check there are no connections
            {
                m_parentNode.m_children.Remove(m_childNode);
                m_childNode.Parent = null;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_parentNode.m_children.Add(m_childNode);
            m_childNode.Parent = m_parentNode;
        }

        private readonly AbstractNode m_parentNode;
        private readonly AbstractNode m_childNode;
    }
}
