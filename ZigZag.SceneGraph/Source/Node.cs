using System.Collections.Generic;
using ZigZag.Math;


namespace ZigZag.SceneGraph
{
    public delegate bool MousePressedDelegate(Vector2 mousePos, int button);
    public delegate void MouseDraggedDelegate(Vector2 mousePos, int button);
    public delegate void MouseReleasedDelegate(Vector2 mousePos, int button);

    public class Node : TreeNode<Node>
    {
        public Node()
        {
            
        }

        public Node(Node parent) : base(parent)
        {
            
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

        public Geometry Geometry
        {
            get;
            set;
        }

        public virtual bool Contains(Vector2 point)
        {
            return BoundingBox.Contains(point - Position);
        }

        public MousePressedDelegate OnMousePressed;
        public MouseDraggedDelegate OnMouseDragged;
        public MouseReleasedDelegate OnMouseReleased;
    }
}
