using System;
using System.Collections.Generic;


namespace ZigZag.Core
{
    public abstract class AbstractNode
    {
        public string Name
        {
            get;
            internal set;
        }

        public AbstractNode Parent
        {
            get;
            internal set;
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
            if (!(child is null))
            {
                return child.IsChildOf(this);
            }
            return false;
        }

        public bool IsChildOf(AbstractNode parent)
        {
            return Parent == parent;
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
            AbstractNode p = Parent;

            while (!(p is null))
            {
                if (p == parent)
                {
                    return true;
                }
                p = p.Parent;
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

        internal readonly List<AbstractNode> m_children = new List<AbstractNode>();
    }
}
