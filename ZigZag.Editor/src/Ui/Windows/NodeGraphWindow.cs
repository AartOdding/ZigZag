using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using ZigZag.SceneGraph;
using ZigZag.Math;


namespace ZigZag.Editor.Ui.Windows
{
    class NodeGraphWindow : DockableWindow
    {
        public NodeGraphWindow(string name) : base(name)
        {
            m_scene = new Scene();
            m_rootNode = new Node();
            m_rootNode.BoundingBox = new Rectangle(0, 0, 300, 300);
            m_scene.RootNode = m_rootNode;

            for (int i = 0; i < 5; ++i)
            {
                var n = new Node();
                m_rootNode.AddChild(n);
                n.BoundingBox = new Rectangle(0, 0, 20, 20);
                n.Position = new Vector2(i * 20, i * 20);
            }
        }

        protected override void DrawImplementation(Style style)
        {
            float x = ContentPos.X;
            float y = ContentPos.Y;
            m_rootNode.BoundingBox = new Rectangle(0, 0, ContentSize.X, ContentSize.Y);

            DrawNode(m_rootNode, ImGui.GetWindowDrawList(), x, y);
        }

        private void DrawNode(Node node, ImDrawListPtr drawList, float tx, float ty)
        {
            var min = new System.Numerics.Vector2(tx + node.Position.X, ty + node.Position.Y);
            var max = new System.Numerics.Vector2(min.X + node.BoundingBox.Width, min.Y + node.BoundingBox.Height);

            drawList.AddRect(min, max, Color.U32(30, 150, 30));

            foreach (var child in node.Children)
            {
                DrawNode(child, drawList, min.X, min.Y);
            }
        }

        private Node m_rootNode;
        internal Scene m_scene;
    }
}
