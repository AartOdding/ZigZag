using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Math;


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

        public void SetMousePosition(float x, float y)
        {
            m_mousePos = new Vector2(x, y);
        }

        public void SetMouseButton(MouseButton button, bool down)
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
            GetNodesAt(RootNode, point, nodes);
            return nodes;
        }

        private static void GetNodesAt(Node currentNode, Vector2 point, List<Node> nodes)
        {
            if (currentNode.Contains(point))
            {
                nodes.Add(currentNode);

                Vector2 pointInCurrentNode = point - currentNode.Position;

                foreach (var child in currentNode.Children)
                {
                    GetNodesAt(child, pointInCurrentNode, nodes);
                }
            }
        }
    }
}
