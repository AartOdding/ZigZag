﻿using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public delegate bool MousePressedDelegate(Vector2 mousePos, int button);
    public delegate void MouseDraggedDelegate(Vector2 mousePos, int button);
    public delegate void MouseReleasedDelegate(Vector2 mousePos, int button);

    public class Node : TreeNode<Node>
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

        public Rectangle BoundingBox
        {
            get;
            set;
        }

        public Vector2 FromParentSpace(Vector2 point)
        {
            if (m_transformIsIdentity)
            {
                return point - Position;
            }
            else
            {
                return (Transform * point) - Position;
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
            return BoundingBox.Contains(point);
        }

        protected virtual bool MousePressEvent(MousePressEvent e)
        {
            return false;
        }

        protected virtual void MouseDragEvent(MouseDragEvent e)
        { }

        protected virtual void MouseReleaseEvent(MouseReleaseEvent e)
        { }

        public MousePressedDelegate OnMousePressed;
        public MouseDraggedDelegate OnMouseDragged;
        public MouseReleasedDelegate OnMouseReleased;

        private Vector2 m_position;
        private Transform2D m_transform;
        private bool m_transformIsIdentity;
    }
}
