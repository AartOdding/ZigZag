using System;
using System.Collections.Generic;


namespace ZigZag.Core
{
    public abstract class AbstractNode
    {
        public class ReparentingException : Exception { }

        public string Name
        {
            get;
            set;
        }

        public AbstractNode Parent
        {
            get
            {
                return m_parent;
            }

            set
            {
                if (IsIndirectParentOf(value))
                {
                    throw new ReparentingException();
                }
                if (!(m_parent is null))
                {
                    m_parent.m_children.Remove(this);
                    m_parent = null;
                }
                if (!(value is null))
                {
                    m_parent = value;
                    value.m_children.Add(this);
                }
            }
        }

        public IEnumerable<AbstractNode> Children
        {
            get
            {
                return m_children;
            }
        }

        public bool IsParentOf(AbstractNode child)
        {
            if (child != null)
            {
                return child.IsChildOf(this);
            }
            return false;
        }

        public bool IsChildOf(AbstractNode parent)
        {
            return m_parent == parent;
        }

        public bool IsIndirectParentOf(AbstractNode child)
        {
            if (!(child is null))
            {
                return child.IsIndirectChildOf(this);
            }
            return false;
        }

        public bool IsIndirectChildOf(AbstractNode parent)
        {
            AbstractNode p = m_parent;

            while (!(p is null))
            {
                if (p == parent)
                {
                    return true;
                }
                p = p.m_parent;
            }
            return p == parent;
        }

        public IEnumerable<T> GetChildren<T>() where T : AbstractNode
        {
            foreach (var child in m_children)
            {
                if (child is T c)
                {
                    yield return c;
                }
            }
        }

        private AbstractNode m_parent;
        private readonly List<AbstractNode> m_children = new List<AbstractNode>();
    }
}
