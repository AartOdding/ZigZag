using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Editor.Math;

namespace ZigZag.Editor.SceneGraph
{
    abstract class Node
    {
        public Node() { }

        public Node(Node parent)
        {
            Parent = parent;
        }

        public Node Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                if (m_parent is not null)
                {
                    m_parent.m_children.Remove(this);
                }
                m_parent = value;

                if (m_parent is not null)
                {
                    m_parent.m_children.Add(this);
                }
            }
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Rectangle BoundingBox
        {
            get;
            set;
        }

        

        private Node m_parent;
        private readonly List<Node> m_children = new List<Node>();
    }
}
