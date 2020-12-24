using System;
using System.Collections.Generic;


namespace ZigZag.Core
{
    public abstract class AbstractNode
    {
        public AbstractNode()
        {
        }

        public AbstractNode(AbstractNode parent)
        {
            Parent = parent;
        }

        public AbstractNode(string name)
        {
            Name = name;
        }

        public AbstractNode(AbstractNode parent, string name)
        {
            Parent = parent;
            Name = name;
        }

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
                if (m_children is null)
                {
                    yield break;
                }
                else
                {
                    foreach (var child in m_children)
                    {
                        yield return child;
                    }
                }
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
            if (m_children is null)
            {
                yield break;
            }
            foreach (var child in m_children)
            {
                if (child is T c)
                {
                    yield return c;
                }
            }
        }

        internal List<AbstractNode> m_children;
    }
}
