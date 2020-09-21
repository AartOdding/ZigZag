using System.Collections.Generic;


namespace ZigZag
{
    public class Object
    {
        public void SetParent(Object parent)
        {
            if (m_parent != null)
            {
                m_parent.m_children.Remove(this);
                m_parent = null;
            }
            if (parent != null)
            {
                m_parent = parent;
                parent.m_children.Add(this);
            }
        }

        public Object GetParent()
        {
            return m_parent;
        }

        public bool IsParentOf(Object child)
        {
            if (child != null)
            {
                return child.IsChildOf(this);
            }
            return false;
        }

        public bool IsChildOf(Object parent)
        {
            return m_parent == parent;
        }

        public bool IsAncestorOf(Object child)
        {
            if (child != null)
            {
                return child.IsDescendantOf(this);
            }
            return false;
        }

        public bool IsDescendantOf(Object parent)
        {
            Object p = m_parent;

            while (p != null)
            {
                if (p == parent)
                {
                    return true;
                }
                p = p.m_parent;
            }
            return p == parent;
        }

        public List<Object> GetChildren()
        {
            return m_children;
        }

        public List<T> GetChildrenOfType<T>() where T : ZigZag.Object
        {
            var children = new List<T>();

            foreach (ZigZag.Object child in m_children)
            {
                if (child is T Tchild)
                {
                    children.Add(Tchild);
                }
            }

            return children;
        }

        private Object m_parent;
        private readonly List<Object> m_children = new List<Object>();
    }
}
