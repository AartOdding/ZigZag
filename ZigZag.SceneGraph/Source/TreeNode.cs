using System;
using System.Collections.Generic;


namespace ZigZag.SceneGraph
{
    public delegate void TreeNodeVisitor<T>(T node);

    public class TreeNode<T> where T : TreeNode<T>
    {
        public TreeNode()
        {

        }

        public TreeNode(T parent)
        {
            if (parent is not null)
            {
                parent.AddChild((T)this);
            }
        }

        public void AddChild(T child)
        {
            if (child is null)
            {
                throw new ArgumentNullException("Child was null.");
            }
            if (child.m_parent is not null)
            {
                throw new Exception("Child already has a parent.");
            }
            m_children.Add(child);
            child.m_parent = (T)this;
        }

        public T Parent
        {
            get
            {
                return m_parent;
            }
        }

        public IReadOnlyList<T> Children
        {
            get
            {
                return m_children;
            }
        }

        // Will always returns false if child is null
        public bool IsParentOf(T child)
        {
            if (child is not null)
            {
                return child.IsChildOf((T)this);
            }
            return false;
        }

        // If parent and m_parent are null returns true
        public bool IsChildOf(T parent)
        {
            return m_parent == parent;
        }

        // Will always returns false if child is null
        public bool IsIndirectParentOf(T child)
        {
            if (child is not null)
            {
                return child.IsIndirectChildOf((T)this);
            }
            return false;
        }

        // Returns true if parent is null
        public bool IsIndirectChildOf(T parent)
        {
            for (var p = m_parent; p is not null; p = p.m_parent)
            {
                if (p == parent)
                {
                    return true;
                }
            }
            return parent is null;
        }

        public void Visit(TreeNodeVisitor<T> visitor)
        {
            visitor((T)this);

            foreach (var child in m_children)
            {
                child.Visit(visitor);
            }
        }

        private T m_parent;
        private readonly List<T> m_children = new List<T>();
    }
}
