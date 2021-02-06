using System.Collections.Generic;
using ZigZag.SceneGraph.Math;


namespace ZigZag.SceneGraph
{
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
    }
}
