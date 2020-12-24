using System.Collections.Generic;
using System.Diagnostics;


namespace ZigZag.Core
{
    public abstract class Node
    {
        public Node()
        {
        }

        public Node(Node parent)
        {
            // No need to check for loops
            Debug.Assert(!(parent is null));
            ParentNode = parent;
            ParentNode.m_childNodes.Add(this);
        }

        public Node(string name)
        {
            Name = name;
        }

        public Node(Node parent, string name)
        {
            // No need to check for loops
            Debug.Assert(!(parent is null));
            ParentNode = parent;
            ParentNode.m_childNodes.Add(this);
            Name = name;
        }

        public string Name
        {
            get;
            internal set;
        }

        public Node ParentNode
        {
            get;
            internal set;
        }

        public IEnumerable<Node> ChildNodes
        {
            get
            {
                if (m_childNodes is null)
                {
                    yield break;
                }
                else
                {
                    foreach (var child in m_childNodes)
                    {
                        yield return child;
                    }
                }
            }
        }

        public abstract void Update();

        public bool IsParentOf(Node child)
        {
            if (!(child is null))
            {
                return child.IsChildOf(this);
            }
            return false;
        }

        public bool IsChildOf(Node parent)
        {
            return ParentNode == parent;
        }

        public bool IsIndirectParentOf(Node child)
        {
            if (!(child is null))
            {
                return child.IsIndirectChildOf(this);
            }
            return false;
        }

        public bool IsIndirectChildOf(Node parent)
        {
            Node p = ParentNode;

            while (!(p is null))
            {
                if (p == parent)
                {
                    return true;
                }
                p = p.ParentNode;
            }
            return p == parent;
        }

        public IEnumerable<T> GetChildren<T>() where T : Node
        {
            if (m_childNodes is null)
            {
                yield break;
            }
            foreach (var child in m_childNodes)
            {
                if (child is T c)
                {
                    yield return c;
                }
            }
        }

        internal List<Node> m_childNodes;
    }
}
