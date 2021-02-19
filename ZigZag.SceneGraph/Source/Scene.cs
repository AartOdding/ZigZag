using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class Scene
    {
        public Scene()
        {
            m_mouseSubscriptions = new Dictionary<MouseButton, HashSet<Node>>
            {
                { MouseButton.Left, new HashSet<Node>() },
                { MouseButton.Middle, new HashSet<Node>() },
                { MouseButton.Right, new HashSet<Node>() }
            };

            m_mouseButtonsDown = new Dictionary<MouseButton, bool>
            {
                { MouseButton.Left, false },
                { MouseButton.Middle, false },
                { MouseButton.Right, false }
            };
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
            var previous = m_mousePos;
            m_mousePos = new Vector2(x, y);

            foreach (var (button, subscribedNodes) in m_mouseSubscriptions)
            {
                foreach (var node in subscribedNodes)
                {
                    var prev = node.MapFromScene(previous);
                    var pos = node.MapFromScene(m_mousePos);
                    node.MouseDragEvent(new MouseDragEvent(pos, pos - prev, button));
                }
            }

        }

        public void SetMouseButtonState(MouseButton button, bool down)
        {
            // If the mouse button was already in the given state, we ignore the event
            // so that widgets will not receive the same event twice in a row.
            if (m_mouseButtonsDown[button] == down)
            {
                return;
            }
            m_mouseButtonsDown[button] = down;

            if (down)
            {
                var intersectingNodes = GetNodesAt(m_mousePos);

                bool consumed = false;

                while (!consumed && intersectingNodes.Count > 0)
                {
                    var node = intersectingNodes[^1];
                    var e = new MousePressEvent(node.MapFromScene(m_mousePos), button);
                    node.MousePressEvent(e);
                    consumed = e.Consume;

                    if (e.Subscribe)
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
        private readonly Dictionary<MouseButton, bool> m_mouseButtonsDown;
        private readonly Dictionary<MouseButton, HashSet<Node>> m_mouseSubscriptions;
    }
}
