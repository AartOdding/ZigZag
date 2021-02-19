using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public delegate void MousePressedDelegate(MousePressEvent e);
    public delegate void MouseDraggedDelegate(MouseDragEvent e);
    public delegate void MouseReleasedDelegate(MouseReleaseEvent e);
    public delegate void MouseWheelDelegate(MouseWheelEvent e);

    public abstract class Node : TreeNode<Node>
    {
        public Node()
        {
            Transform = Transform2D.Identity;
        }

        public Node(Node parent) : base(parent)
        {
            Transform = Transform2D.Identity;
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
                UpdateTransforms();
            }
        }

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
                UpdateTransforms();
            }
        }

        public bool ChangedThisFrame
        {
            get;
            internal set;
        }

        public Transform2D GetNodeTransform()
        {
            return m_toParentTransform;
        }

        public Vector2 MapToParent(Vector2 point)
        {
            return m_toParentTransform * point;
        }

        public Vector2 MapFromParent(Vector2 point)
        {
            return m_fromParentTransform * point;
        }

        public Vector2 MapToScene(Vector2 point)
        {
            var node = this;

            while (node is not null)
            {
                point = node.MapToParent(point);
                node = node.Parent;
            }
            return point;
        }

        public Vector2 MapFromScene(Vector2 point)
        {
            var ancestors = GetAncestors(true);

            for(int i = ancestors.Count - 1; i >= 0; --i)
            {
                point = ancestors[i].MapFromParent(point);
            }
            return point;
        }

        // point given in local coordinates
        public abstract bool Contains(Vector2 point);

        // point given in local coordinates
        public virtual List<Node> GetChildrenAt(Vector2 point)
        {
            List<Node> result = new List<Node>();

            foreach (var child in Children)
            {
                if (child.Contains(child.MapFromParent(point)))
                {
                    result.Add(child);
                }
            }
            return result;
        }

        protected internal virtual void MousePressEvent(MousePressEvent e)
        {
            if (OnMousePressed is not null)
            {
                OnMousePressed(e);
            }
        }

        protected internal virtual void MouseDragEvent(MouseDragEvent e)
        {
            if (OnMouseDragged is not null)
            {
                OnMouseDragged(e);
            }
        }

        protected internal virtual void MouseReleaseEvent(MouseReleaseEvent e)
        {
            if (OnMouseReleased is not null)
            {
                OnMouseReleased(e);
            }
        }

        protected internal virtual void MouseWheelEvent(MouseWheelEvent e)
        {
            if (OnMouseWheel is not null)
            {
                OnMouseWheel(e);
            }
        }

        public MousePressedDelegate OnMousePressed;
        public MouseDraggedDelegate OnMouseDragged;
        public MouseReleasedDelegate OnMouseReleased;
        public MouseWheelDelegate OnMouseWheel;

        private void UpdateTransforms()
        {
            m_toParentTransform = m_transform * Transform2D.CreateTranslation(Position);
            m_fromParentTransform = m_toParentTransform.Inverse();
        }

        private Vector2 m_position;
        private Transform2D m_transform;
        private Transform2D m_fromParentTransform;
        private Transform2D m_toParentTransform;
    }
}
