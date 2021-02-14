using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class Scene
    {
        public Node RootNode
        {
            get;
            set;
        }

        private Vector2 m_mousePos;

        public void BeginNewFrame()
        {
            RootNode.Visit(node => node.ChangedThisFrame = false);
        }

        public void SetMousePosition(float x, float y)
        {
            m_mousePos = new Vector2(x, y);

            var intersectingNodes = GetNodesAt(m_mousePos);
            foreach (var n in intersectingNodes)
            {
                Console.Write(n);
                Console.Write("   ");
            }
            Console.WriteLine();
        }

        public void SetMouseButtonState(MouseButton button, bool down)
        {
            if (down)
            {
                var intersectingNodes = GetNodesAt(m_mousePos);
                foreach(var n in intersectingNodes)
                {
                    Console.Write(n);
                    Console.Write("   ");
                }
                Console.WriteLine();
            }
        }

        public List<Node> GetNodesAt(Vector2 point)
        {
            List<Node> nodes = new List<Node>();
            // map point?
            GetNodesAt(RootNode, RootNode.FromParentSpace(point), nodes);
            return nodes;
        }

        private static void GetNodesAt(Node currentNode, Vector2 point, List<Node> nodes)
        {
            if (currentNode.Contains(point))
            {
                nodes.Add(currentNode);

                foreach (var child in currentNode.Children)
                {
                    Vector2 pointInChild = child.FromParentSpace(point);
                    GetNodesAt(child, pointInChild, nodes);
                }
            }
        }
    }
}
