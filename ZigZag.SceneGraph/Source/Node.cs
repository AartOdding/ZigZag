using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public delegate void MouseButtonPressDelegate(MouseButtonPressEvent e);
    public delegate void MouseButtonDragDelegate(MouseButtonDragEvent e);
    public delegate void MouseButtonReleaseDelegate(MouseButtonReleaseEvent e);
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

        internal void PerformMouseButtonPressEvent(MouseButtonPressEvent e)
        {
            var handler = MouseButtonPressEvent;

            if (handler is not null)
            {
                e.State = EventState.ImplicitlyAccepted;
                handler(e);
            }
            else
            {
                e.State = EventState.ImplicitlyDeclined;
            }
        }

        internal void PerformMouseButtonDragEvent(MouseButtonDragEvent e)
        {
            var handler = MouseButtonDragEvent;

            if (handler is not null)
            {
                handler(e);
            }
        }

        internal void PerformMouseButtonReleaseEvent(MouseButtonReleaseEvent e)
        {
            var handler = MouseButtonReleaseEvent;

            if (handler is not null)
            {
                handler(e);
            }
        }

        internal void PerformMouseWheelEvent(MouseWheelEvent e)
        {
            var handler = MouseWheelEvent;

            if (handler is not null)
            {
                e.State = EventState.ImplicitlyAccepted;
                handler(e);
            }
            else
            {
                e.State = EventState.ImplicitlyDeclined;
            }
        }

        public event MouseButtonPressDelegate MouseButtonPressEvent;
        public event MouseButtonDragDelegate MouseButtonDragEvent;
        public event MouseButtonReleaseDelegate MouseButtonReleaseEvent;
        public event MouseWheelDelegate MouseWheelEvent;

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
