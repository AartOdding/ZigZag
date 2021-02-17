using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class Scene
    {
        public Scene()
        {
            m_mouseSubscriptions = new Dictionary<MouseButton, List<Node>>();
            m_mouseSubscriptions.Add(MouseButton.Left, new List<Node>());
            m_mouseSubscriptions.Add(MouseButton.Middle, new List<Node>());
            m_mouseSubscriptions.Add(MouseButton.Right, new List<Node>());
        }

        public Node RootNode
        {
            get;
            set;
        }

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

                bool consumed = false;
                
                while (!consumed && intersectingNodes.Count > 0)
                {
                    var node = intersectingNodes[^1];
                    var e = new MousePressEvent(node.MapFromScene(m_mousePos), button);
                    node.MousePressEvent(e, out consumed, out bool subscribe);

                    if (subscribe)
                    {
                        m_mouseSubscriptions[button].Add(node);
                    }
                }

                if (!consumed)
                {
                    Console.Write("Event not consumed");
                }
                else
                {
                    Console.WriteLine("Consumed!");
                }
            }
        }

        public List<Node> GetNodesAt(Vector2 point)
        {
            List<Node> nodes = new List<Node>();
            var pointInRootNode = RootNode.MapFromParent(point);

            if (RootNode.Contains(pointInRootNode))
            {
                nodes.Add(RootNode);
                GetChildrenAt(RootNode, pointInRootNode, nodes);
            }
            return nodes;
        }

        private static void GetChildrenAt(Node node, Vector2 point, List<Node> nodes)
        {
            var intersectingChildren = node.GetChildrenAt(point);

            foreach(var child in intersectingChildren)
            {
                nodes.Add(child);
                GetChildrenAt(child, child.MapFromParent(point), nodes);
            }
        }

        private Vector2 m_mousePos;
        private readonly Dictionary<MouseButton, List<Node>> m_mouseSubscriptions;
    }
}
