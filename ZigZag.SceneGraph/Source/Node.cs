using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public delegate void MousePressedDelegate(MousePressEvent e, out bool consume, out bool subscribe);
    public delegate void MouseDraggedDelegate(MouseDragEvent e);
    public delegate void MouseReleasedDelegate(MouseReleaseEvent e);

    public abstract class Node : TreeNode<Node>
    {
        public Node()
        {
            Position = new Vector2(0, 0);
            Transform = Transform2D.Identity;
        }

        public Node(Node parent) : base(parent)
        {
            Position = new Vector2(0, 0);
            Transform = Transform2D.Identity;
        }

        public bool ChangedThisFrame
        {
            get;
            internal set;
        }

        // Position in the parent's coordinate space
        public Vector2 Position
        {
            get
            {
                return m_position;
            }
            set
            {
                ChangedThisFrame |= (m_position != value);
                m_position = value; 
            }
        }

        // The transform to move from the parent's to this coordinate space.
        public Transform2D Transform
        {
            get
            {
                return m_transform;
            }
            set
            {
                ChangedThisFrame |= (m_transform != value);
                m_transform = value;
                m_transformIsIdentity = m_transform.IsIdentity;
            }
        }

        public Transform2D GetNodeTransform()
        {
            return Transform * Transform2D.CreateTranslation(Position);
        }

        // Should return the nodes Bounding Box in local space.
        public abstract Rectangle GetBoundingBox();

        public Vector2 FromParentSpace(Vector2 point)
        {
            if (m_transformIsIdentity)
            {
                return point - Position;
            }
            else
            {
                return Transform * (point - Position);
            }
        }

        public Vector2 ToParentSpace(Vector2 point)
        {
            // needs inverse
            throw new System.NotImplementedException();
        }

        // point given in local coordinates
        public virtual bool Contains(Vector2 point)
        {
            return GetBoundingBox().Contains(point);
        }

        protected virtual void MousePressEvent(MousePressEvent e, out bool consume, out bool subscribe)
        {
            if (OnMousePressed is not null)
            {
                OnMousePressed(e, out consume, out subscribe);
            }
            else
            {
                consume = false;
                subscribe = false;
            }
        }

        protected virtual void MouseDragEvent(MouseDragEvent e)
        {
            if (OnMouseDragged is not null)
            {
                OnMouseDragged(e);
            }
        }

        protected virtual void MouseReleaseEvent(MouseReleaseEvent e)
        {
            if (OnMouseReleased is not null)
            {
                OnMouseReleased(e);
            }
        }

        public MousePressedDelegate OnMousePressed;
        public MouseDraggedDelegate OnMouseDragged;
        public MouseReleasedDelegate OnMouseReleased;

        private Vector2 m_position;
        private Transform2D m_transform;
        private bool m_transformIsIdentity;
    }
}
