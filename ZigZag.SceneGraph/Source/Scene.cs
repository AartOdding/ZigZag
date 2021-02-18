using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class Scene
    {
        public Scene()
        {
            m_mouseSubscriptions = new Dictionary<MouseButton, HashSet<Node>>();
            m_mouseSubscriptions.Add(MouseButton.Left, new HashSet<Node>());
            m_mouseSubscriptions.Add(MouseButton.Middle, new HashSet<Node>());
            m_mouseSubscriptions.Add(MouseButton.Right, new HashSet<Node>());
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
                    intersectingNodes.RemoveAt(intersectingNodes.Count - 1);
                }
            }
            else
            {
                foreach (var node in m_mouseSubscriptions[button])
                {
                    var e = new MouseReleaseEvent(node.MapFromScene(m_mousePos), button);
                    node.MouseReleaseEvent(e);
                }
                m_mouseSubscriptions[button].Clear();
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
        private readonly Dictionary<MouseButton, HashSet<Node>> m_mouseSubscriptions;
    }
}
