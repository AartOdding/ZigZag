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

        public void MouseMovement(float x, float y)
        {
            var previous = m_mousePos;
            m_mousePos = new Vector2(x, y);

            foreach (var (button, subscribedNodes) in m_mouseSubscriptions)
            {
                foreach (var node in subscribedNodes)
                {
                    var prev = node.MapFromScene(previous);
                    var pos = node.MapFromScene(m_mousePos);
                    node.PerformMouseButtonDragEvent(new MouseButtonDragEvent(pos, pos - prev, button));
                }
            }
        }

        public void MouseButtonPress(MouseButton button)
        {
            // If the mouse button was already in the press state, we ignore the 
            // event so that widgets will not receive the same event twice in a row.
            if (m_mouseButtonsDown[button]) return;
            m_mouseButtonsDown[button] = true;

            var intersectingNodes = GetNodesAt(m_mousePos);

            for (int i = intersectingNodes.Count - 1; i >= 0; --i)
            {
                var node = intersectingNodes[i];
                var e = new MouseButtonPressEvent(node.MapFromScene(m_mousePos), button);
                node.PerformMouseButtonPressEvent(e);

                if (e.State == EventState.Accepted || e.State == EventState.ImplicitlyAccepted)
                {
                    m_mouseSubscriptions[button].Add(node);
                    break;
                }
            }

            // check for double click
        }

        public void MouseButtonRelease(MouseButton button)
        {
            // If the mouse button was already in the released state, we ignore the 
            // event so that widgets will not receive the same event twice in a row.
            if (!m_mouseButtonsDown[button]) return;
            m_mouseButtonsDown[button] = false;

            foreach (var node in m_mouseSubscriptions[button])
            {
                var e = new MouseButtonReleaseEvent(node.MapFromScene(m_mousePos), button);
                node.PerformMouseButtonReleaseEvent(e);
            }
            m_mouseSubscriptions[button].Clear();
        }

        public void MouseWheel(float delta)
        {
            var intersectingNodes = GetNodesAt(m_mousePos);

            for (int i = intersectingNodes.Count - 1; i >= 0; --i)
            {
                var node = intersectingNodes[i];
                var e = new MouseWheelEvent(delta, node.MapFromScene(m_mousePos));
                node.PerformMouseWheelEvent(e);

                if (e.State == EventState.Accepted || e.State == EventState.ImplicitlyAccepted)
                {
                    break;
                }
            }
        }

        public List<Node> GetNodesAt(Vector2 point)
        {
            List<Node> nodes = new List<Node>();
            GetChildrenAt(RootNode, RootNode.MapFromParent(point), nodes);
            return nodes;
        }

        private static void GetChildrenAt(Node node, Vector2 point, List<Node> nodes)
        {
            if (node.Contains(point))
            {
                nodes.Add(node);

                foreach (var child in node.GetChildrenAt(point))
                {
                    GetChildrenAt(child, child.MapFromParent(point), nodes);
                }
            }
        }

        private Vector2 m_mousePos;
        private readonly Dictionary<MouseButton, bool> m_mouseButtonsDown;
        private readonly Dictionary<MouseButton, HashSet<Node>> m_mouseSubscriptions;
    }
}
