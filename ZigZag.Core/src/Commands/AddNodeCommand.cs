

namespace ZigZag.Core.Commands
{
    public class AddNodeCommand : AbstractCommand
    {
        public AddNodeCommand(AbstractNode parentNode, AbstractNode childNodeToAdd)
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
            if (m_childNode.Parent is null &&
                !m_parentNode.m_children.Contains(m_childNode) &&
                !m_parentNode.IsIndirectChildOf(m_childNode))
            {
                m_parentNode.m_children.Add(m_childNode);
                m_childNode.Parent = m_parentNode;
            }
            else
            {
                throw new CommandException();
            }
        }

        internal override void Undo()
        {
            m_parentNode.m_children.Remove(m_childNode);
            m_childNode.Parent = null;
        }

        private readonly AbstractNode m_parentNode;
        private readonly AbstractNode m_childNode;
    }
}
