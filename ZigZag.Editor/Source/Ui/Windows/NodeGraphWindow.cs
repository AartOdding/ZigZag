using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using ZigZag.SceneGraph;
using ZigZag.SceneGraph.Widgets;
using ZigZag.Mathematics;


namespace ZigZag.Editor.Ui.Windows
{
    class NodeGraphWindow : DockableWindow
    {
        public NodeGraphWindow(string name) : base(name)
        {
            m_scene = new Scene();
            m_renderer = new SceneGraph.SceneRenderer(m_scene, true);
            m_rootNode = new InfinitePlane();
            m_scene.RootNode = m_rootNode;

            for (int i = 0; i < 5; ++i)
            {
                var n = new SceneGraph.CircleTestWidget(m_rootNode);
                n.Position = new Vector2(i * 80, i * 80);
            }

            m_squareNode = new SceneGraph.SquareTestWidget(m_rootNode);
            m_squareNode.Position = new Vector2(300, 300);
        }

        protected override void DrawImplementation(Style style)
        { }

        public void Render(float windowWidth, float windowHeight)
        {
            m_renderer.Render(new Rectangle(ContentPos.X, ContentPos.Y, ContentSize.X, ContentSize.Y), windowWidth, windowHeight);
        }

        private float rotation = 0;
        private InfinitePlane m_rootNode;
        private GeometryNode m_squareNode;
        internal Scene m_scene;
        private SceneGraph.SceneRenderer m_renderer;
    }
}
